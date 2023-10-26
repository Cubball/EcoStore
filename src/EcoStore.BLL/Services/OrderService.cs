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

    public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IPaymentRepository paymentRepository,
            IClock clock,
            IValidator<CreateOrderDTO> createOrderValidator,
            IValidator<UpdateOrderStatusDTO> updateOrderStatusValidator,
            IValidator<UpdateOrderTrackingNumberDTO> updateOrderTrackingNumberValidator)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _paymentRepository = paymentRepository;
        _clock = clock;
        _createOrderValidator = createOrderValidator;
        _updateOrderStatusValidator = updateOrderStatusValidator;
        _updateOrderTrackingNumberValidator = updateOrderTrackingNumberValidator;
    }

    public async Task<int> CreateOrderAsync(CreateOrderDTO orderDTO)
    {
        await _createOrderValidator.ValidateAsync(orderDTO);
        var order = orderDTO.ToEntity();
        await FillOrderDetails(orderDTO, order);

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
        try
        {
            return (await _orderRepository.GetOrderByIdAsync(id)).ToDTO();
        }
        catch (EntityNotFoundException e)
        {
            throw new ObjectNotFoundException(e.Message, e);
        }
    }


    public async Task DeleteOrderAsync(int id)
    {
        string? paymentId;
        try
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            paymentId = order.Payment?.Id;
        }
        catch (EntityNotFoundException e)
        {
            throw new ObjectNotFoundException(e.Message, e);
        }

        try
        {
            await _orderRepository.DeleteOrderAsync(id);
            if (paymentId is not null)
            {
                await _paymentRepository.DeletePaymentAsync(paymentId);
            }
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }

    public async Task<IEnumerable<OrderDTO>> GetOrdersAsync()
    {
        return (await _orderRepository.GetOrdersAsync()).Select(o => o.ToDTO());
    }

    public async Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(string userId)
    {
        return (await _orderRepository.GetOrdersByUserIdAsync(userId)).Select(o => o.ToDTO());
    }

    public async Task UpdateOrderStatusAsync(UpdateOrderStatusDTO orderDTO)
    {
        await _updateOrderStatusValidator.ValidateAsync(orderDTO);
        var orderStatus = Enum.Parse<OrderStatus>(orderDTO.OrderStatus);
        var order = await TryGetOrderAsync(orderDTO.Id);
        await UpdateOrderStatusAsync(order, orderStatus);
    }

    public async Task UpdateOrderTrackingNumberAsync(UpdateOrderTrackingNumberDTO orderDTO)
    {
        await _updateOrderTrackingNumberValidator.ValidateAsync(orderDTO);
        var order = await TryGetOrderAsync(orderDTO.Id);

        order.TrackingNumber = orderDTO.TrackingNumber;
        try
        {
            await _orderRepository.UpdateOrderAsync(order);
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }

    public async Task CancelOrderAsUserAsync(int id)
    {
        var order = await TryGetOrderAsync(id);
        await UpdateOrderStatusAsync(order, OrderStatus.CancelledByUser);
        await AddBackProductStockFromCancelledOrder(order);
    }

    // TODO : refactor
    public async Task CancelOrderAsAdminAsync(int id)
    {
        var order = await TryGetOrderAsync(id);
        await UpdateOrderStatusAsync(order, OrderStatus.CancelledByAdmin);
        await AddBackProductStockFromCancelledOrder(order);
    }

    // TODO Check for status before cancelling and so on, StatusChangedDate
    private async Task UpdateOrderStatusAsync(Order order, OrderStatus status)
    {
        order.OrderStatus = status;
        order.StatusChangedDate = new DateTime(_clock.UtcNow.Ticks, DateTimeKind.Utc);
        try
        {
            await _orderRepository.UpdateOrderAsync(order);
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }

    // TODO: fix this
    private async Task AddBackProductStockFromCancelledOrder(Order order)
    {
        if (order.Payment is not null)
        {
            await RefundChargeAsync(order.Payment.Id);
        }

        foreach (var orderedProduct in order.OrderedProducts)
        {
            var product = await _productRepository.GetProductByIdAsync(orderedProduct.ProductId);
            product.Stock += orderedProduct.Quantity;
            // TODO: change repo update logic?
            await _productRepository.UpdateProductAsync(product);
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

    private async Task FillOrderDetails(CreateOrderDTO orderDTO, Order order)
    {
        var utcNow = new DateTime(_clock.UtcNow.Ticks, DateTimeKind.Utc);
        order.OrderDate = utcNow;
        order.OrderStatus = OrderStatus.New;
        order.StatusChangedDate = utcNow;
        order.PaymentMethod = Enum.Parse<DAL.Entities.PaymentMethod>(orderDTO.PaymentMethod);
        order.ShippingMethod = Enum.Parse<ShippingMethod>(orderDTO.ShippingMethod);
        await FillOrderedProductsDetails(order);
    }

    private async Task FillOrderedProductsDetails(Order order)
    {
        foreach (var orderedProduct in order.OrderedProducts)
        {
            var product = await _productRepository.GetProductByIdAsync(orderedProduct.ProductId);
            orderedProduct.ProductPrice = product.Price;
            product.Stock -= orderedProduct.Quantity;
            await _productRepository.UpdateProductAsync(product);
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
        if (!charge.Paid)
        {
            throw new PaymentFailedException($"Оплата не вдалася. Код помилки: {charge.FailureCode}");
        }

        return new Payment
        {
            Id = charge.Id,
            Created = charge.Created,
            Amount = (int)charge.Amount,
            Currency = charge.Currency,
        };
    }

    private async Task<Order> TryGetOrderAsync(int id)
    {
        try
        {
            return await _orderRepository.GetOrderByIdAsync(id);
        }
        catch (EntityNotFoundException e)
        {
            throw new ObjectNotFoundException(e.Message, e);
        }
    }
}