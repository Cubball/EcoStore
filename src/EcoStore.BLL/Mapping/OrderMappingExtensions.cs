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
}