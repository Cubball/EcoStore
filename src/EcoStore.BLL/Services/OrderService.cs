using System.Linq.Expressions;

using EcoStore.BLL.DTO;
using EcoStore.BLL.Infrastructure;
using EcoStore.BLL.Mapping;
using EcoStore.BLL.Services.Exceptions;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.BLL.Validation.Interfaces;
using EcoStore.DAL.Entities;
using EcoStore.DAL.Repositories.Exceptions;
using EcoStore.DAL.Repositories.Interfaces;

using Stripe;

namespace EcoStore.BLL.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IClock _clock;
    private readonly IValidator<CreateOrderDTO> _createOrderValidator;
    private readonly IValidator<UpdateOrderStatusDTO> _updateOrderStatusValidator;
    private readonly IValidator<UpdateOrderTrackingNumberDTO> _updateOrderTrackingNumberValidator;
    private readonly IValidator<CancelOrderByAdminDTO> _cancelOrderByAdminValidator;
    private readonly IValidator<CancelOrderByUserDTO> _cancelOrderByUserValidator;

    private Expression<Func<Order, object>>? _orderBy;

    public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IPaymentRepository paymentRepository,
            IClock clock,
            IValidator<CreateOrderDTO> createOrderValidator,
            IValidator<UpdateOrderStatusDTO> updateOrderStatusValidator,
            IValidator<UpdateOrderTrackingNumberDTO> updateOrderTrackingNumberValidator,
            IValidator<CancelOrderByAdminDTO> cancelOrderByAdminValidator,
            IValidator<CancelOrderByUserDTO> cancelOrderByUserValidator)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _paymentRepository = paymentRepository;
        _clock = clock;
        _createOrderValidator = createOrderValidator;
        _updateOrderStatusValidator = updateOrderStatusValidator;
        _updateOrderTrackingNumberValidator = updateOrderTrackingNumberValidator;
        _cancelOrderByAdminValidator = cancelOrderByAdminValidator;
        _cancelOrderByUserValidator = cancelOrderByUserValidator;
    }

    public async Task<int> CreateOrderAsync(CreateOrderDTO orderDTO)
    {
        await _createOrderValidator.ValidateAsync(orderDTO);
        var order = orderDTO.ToEntity();
        await SetOrderDetailsAsync(order);

        if (order.PaymentMethod == DAL.Entities.PaymentMethod.Card)
        {
            order.Payment = await GetPaymentForOrderAsync(order, orderDTO.StripeToken!);
        }

        try
        {
            return await _orderRepository.AddOrderAsync(order);
        }
        catch (RepositoryException e)
        {
            if (order.PaymentMethod == DAL.Entities.PaymentMethod.Card)
            {
                await RefundChargeAsync(order.Payment!.Id);
            }

            throw new ServiceException(e.Message, e);
        }
    }

    public async Task<OrderDTO> GetOrderAsync(int id)
    {
        return (await TryGetOrder(id)).ToDTO();
    }


    public async Task DeleteOrderAsync(int id)
    {
        var order = await TryGetOrder(id);
        try
        {
            await _orderRepository.DeleteOrderAsync(id);
            if (order.PaymentId is not null)
            {
                await _paymentRepository.DeletePaymentAsync(order.PaymentId);
            }
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }

    public async Task<IEnumerable<OrderDTO>> GetOrdersAsync(
            int pageNumber,
            int pageSize,
            string? userId = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
    {
        _orderBy ??= o => o.OrderDate;
        var skip = (pageNumber - 1) * pageSize;
        var orders = await _orderRepository.GetOrdersAsync(
                skip: skip,
                count: pageSize,
                predicate: GetOrderPredicate(userId, startDate, endDate),
                orderBy: _orderBy,
                descending: true);
        return orders.Select(o => o.ToDTO());
    }

    public async Task<int> GetOrderCountAsync(
            string? userId = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
    {
        return await _orderRepository.GetOrdersCountAsync(GetOrderPredicate(userId, startDate, endDate));
    }

    private static Expression<Func<Order, bool>>? GetOrderPredicate(string? userId, DateTime? startDate, DateTime? endDate)
    {
        Expression<Func<Order, bool>>? predicate = null;
        if (userId is not null)
        {
            predicate = PredicateBuilder.Combine(predicate, o => o.UserId == userId);
        }

        if (startDate is not null)
        {
            predicate = PredicateBuilder.Combine(predicate, o => o.OrderDate >= startDate);
        }

        if (endDate is not null)
        {
            predicate = PredicateBuilder.Combine(predicate, o => o.OrderDate <= endDate);
        }

        return predicate;
    }

    public async Task UpdateOrderStatusAsync(UpdateOrderStatusDTO orderDTO)
    {
        await _updateOrderStatusValidator.ValidateAsync(orderDTO);
        await UpdateOrderStatusAsync(orderDTO.Id, orderDTO.OrderStatus.ToEntity());
    }

    public async Task UpdateOrderTrackingNumberAsync(UpdateOrderTrackingNumberDTO orderDTO)
    {
        await _updateOrderTrackingNumberValidator.ValidateAsync(orderDTO);
        try
        {
            await _orderRepository.UpdateOrderAsync(orderDTO.Id,
                    o => o.TrackingNumber = orderDTO.TrackingNumber);
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }

    public async Task CancelOrderByUserAsync(CancelOrderByUserDTO orderDTO)
    {
        await _cancelOrderByUserValidator.ValidateAsync(orderDTO);
        await UpdateOrderStatusAsync(orderDTO.Id, OrderStatus.CancelledByUser);
        await CancelOrder(orderDTO.Id);
    }

    public async Task CancelOrderByAdminAsync(CancelOrderByAdminDTO orderDTO)
    {
        await _cancelOrderByAdminValidator.ValidateAsync(orderDTO);
        await UpdateOrderStatusAsync(orderDTO.Id, OrderStatus.CancelledByAdmin);
        await CancelOrder(orderDTO.Id);
    }

    private async Task CancelOrder(int orderId)
    {
        var order = await TryGetOrder(orderId);
        if (order.Payment is not null)
        {
            await RefundChargeAsync(order.Payment.Id);
        }

        foreach (var orderedProduct in order.OrderedProducts)
        {
            await _productRepository.UpdateProductAsync(orderedProduct.ProductId,
                    p => p.Stock += orderedProduct.Quantity);
        }
    }

    private async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        try
        {
            await _orderRepository.UpdateOrderAsync(orderId, o =>
            {
                o.StatusChangedDate = new DateTime(_clock.UtcNow.Ticks, DateTimeKind.Utc);
                o.OrderStatus = status;
            });
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }

    private static async Task RefundChargeAsync(string chargeId)
    {
        var refundCreateOptions = new RefundCreateOptions
        {
            Charge = chargeId,
        };

        var refundService = new RefundService();
        await refundService.CreateAsync(refundCreateOptions);
    }

    private async Task SetOrderDetailsAsync(Order order)
    {
        var utcNow = new DateTime(_clock.UtcNow.Ticks, DateTimeKind.Utc);
        order.OrderDate = utcNow;
        order.OrderStatus = OrderStatus.New;
        order.StatusChangedDate = utcNow;
        await SetOrderedProductsDetailsAsync(order);
        await ReduceProductsStockAsync(order);
    }

    private async Task SetOrderedProductsDetailsAsync(Order order)
    {
        foreach (var orderedProduct in order.OrderedProducts)
        {
            var product = await _productRepository.GetProductByIdAsync(orderedProduct.ProductId);
            orderedProduct.ProductPrice = product.Price;
        }
    }

    private async Task ReduceProductsStockAsync(Order order)
    {
        foreach (var orderedProduct in order.OrderedProducts)
        {
            await _productRepository.UpdateProductAsync(orderedProduct.ProductId,
                    p => p.Stock -= orderedProduct.Quantity);
        }
    }

    private async Task<Order> TryGetOrder(int orderId)
    {
        try
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }
        catch (EntityNotFoundException e)
        {
            throw new ObjectDisposedException(e.Message, e);
        }
    }

    private static async Task<Payment> GetPaymentForOrderAsync(Order order, string stripeToken)
    {
        var chargeCreateOptions = new ChargeCreateOptions
        {
            Amount = (long)(order.TotalPrice * 100),
            Currency = "uah",
            Source = stripeToken,
        };
        var chargeService = new ChargeService();
        var charge = await chargeService.CreateAsync(chargeCreateOptions);

        return charge.Paid
            ? new Payment
            {
                Id = charge.Id,
                Created = charge.Created,
                Amount = (int)charge.Amount,
                Currency = charge.Currency,
            }
            : throw new PaymentFailedException($"Оплата не вдалася. Код помилки: {charge.FailureCode}");
    }
}