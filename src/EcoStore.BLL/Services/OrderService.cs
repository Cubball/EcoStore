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
    private readonly IClock _clock;
    private readonly IValidator<CreateOrderDTO> _createOrderValidator;

    public OrderService(IOrderRepository orderRepository,
            IProductRepository productRepository,
            IClock clock,
            IValidator<CreateOrderDTO> createOrderValidator)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _clock = clock;
        _createOrderValidator = createOrderValidator;
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

    public Task<OrderDTO> GetOrderAsync(int id)
    {
        throw new NotImplementedException();
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

    private static async Task RefundChargeAsync(string chargeId)
    {
        var refundCreateOptions = new RefundCreateOptions
        {
            Charge = chargeId,
        };

        var refundService = new RefundService();
        await refundService.CreateAsync(refundCreateOptions);
    }
}