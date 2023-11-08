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
            OrderedProducts = orderDTO.OrderedProducts.Select(op => op.ToViewModel()),
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
}