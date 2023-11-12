using EcoStore.BLL.DTO;
using EcoStore.Presentation.ViewModels;

namespace EcoStore.Presentation.Mapping;

public static class OrderMappingExtensions
{
    public static OrderViewModel ToViewModel(this OrderDTO orderDTO)
    {
        return new OrderViewModel
        {
            Id = orderDTO.Id,
            User = orderDTO.User.ToViewModel(),
            OrderedProducts = orderDTO.OrderedProducts.Select(op => op.ToViewModel()).ToList(),
            OrderDate = orderDTO.OrderDate,
            OrderStatus = orderDTO.OrderStatus.ToViewModel(),
            StatusChangedDate = orderDTO.StatusChangedDate,
            PaymentMethod = orderDTO.PaymentMethod.ToViewModel(),
            Payment = orderDTO.Payment?.ToViewModel(),
            ShippingAddress = orderDTO.ShippingAddress,
            ShippingMethod = orderDTO.ShippingMethod.ToViewModel(),
            TrackingNumber = orderDTO.TrackingNumber,
        };
    }

    public static OrderedProductViewModel ToViewModel(this OrderedProductDTO orderedProductDTO)
    {
        return new OrderedProductViewModel
        {
            Product = orderedProductDTO.Product?.ToViewModel(),
            Quantity = orderedProductDTO.Quantity,
            ProductPrice = orderedProductDTO.ProductPrice,
        };
    }

    public static CreateOrderDTO ToDTO(this CreateOrderViewModel createOrderViewModel)
    {
        return new CreateOrderDTO
        {
            StripeToken = createOrderViewModel.StripeToken,
            PaymentMethod = createOrderViewModel.PaymentMethod.ToDTO(),
            ShippingAddress = createOrderViewModel.ShippingAddress,
            ShippingMethod = createOrderViewModel.ShippingMethod.ToDTO(),
            OrderedProducts = createOrderViewModel.Cart.CartItems!.Select(ci => new CreateOrderedProductDTO
            {
                ProductId = ci.Product.Id,
                Quantity = ci.Quantity,
            }),
        };
    }
}