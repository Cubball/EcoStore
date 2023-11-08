using EcoStore.BLL.DTO;
using EcoStore.DAL.Entities;

namespace EcoStore.BLL.Mapping;

public static class OrderMappingExtenstions
{
    public static Order ToEntity(this CreateOrderDTO orderDTO)
    {
        var order = new Order
        {
            UserId = orderDTO.UserId,
            ShippingAddress = orderDTO.ShippingAddress,
            PaymentMethod = orderDTO.PaymentMethod.ToEntity(),
            ShippingMethod = orderDTO.ShippingMethod.ToEntity(),
        };
        foreach (var orderedProduct in orderDTO.OrderedProducts)
        {
            order.OrderedProducts.Add(orderedProduct.ToEntity());
        }

        return order;
    }

    public static OrderedProduct ToEntity(this CreateOrderedProductDTO orderedProductDTO)
    {
        return new OrderedProduct
        {
            ProductId = orderedProductDTO.ProductId,
            Quantity = orderedProductDTO.Quantity,
        };
    }

    public static OrderDTO ToDTO(this Order order)
    {
        return new OrderDTO
        {
            Id = order.Id,
            User = order.User.ToDTO(),
            OrderedProducts = order.OrderedProducts.Select(op => op.ToDTO()),
            OrderDate = order.OrderDate,
            OrderStatus = order.OrderStatus.ToDTO(),
            StatusChangedDate = order.StatusChangedDate,
            PaymentMethod = order.PaymentMethod.ToDTO(),
            Payment = order.Payment?.ToDTO(),
            ShippingAddress = order.ShippingAddress,
            ShippingMethod = order.ShippingMethod.ToDTO(),
            TrackingNumber = order.TrackingNumber,
        };
    }

    public static OrderedProductDTO ToDTO(this OrderedProduct orderedProduct)
    {
        return new OrderedProductDTO
        {
            Product = orderedProduct.Product?.ToDTO(),
            Quantity = orderedProduct.Quantity,
            ProductPrice = orderedProduct.ProductPrice,
        };
    }
}