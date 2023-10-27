using EcoStore.BLL.DTO;
using EcoStore.DAL.Entities;

namespace EcoStore.BLL.Mapping;

public static class EnumsMappingExtensions
{
    public static PaymentMethod ToEntity(this PaymentMethodDTO paymentMethodDTO)
    {
        return paymentMethodDTO switch
        {
            PaymentMethodDTO.Cash => PaymentMethod.Cash,
            PaymentMethodDTO.Card => PaymentMethod.Card,
            _ => throw new ArgumentOutOfRangeException(nameof(paymentMethodDTO), paymentMethodDTO, null)
        };
    }

    public static ShippingMethod ToEntity(this ShippingMethodDTO shippingMethodDTO)
    {
        return shippingMethodDTO switch
        {
            ShippingMethodDTO.NovaPoshta => ShippingMethod.NovaPoshta,
            ShippingMethodDTO.UkrPoshta => ShippingMethod.UkrPoshta,
            _ => throw new ArgumentOutOfRangeException(nameof(shippingMethodDTO), shippingMethodDTO, null)
        };
    }

    public static OrderStatus ToEntity(this OrderStatusDTO orderStatusDTO)
    {
        return orderStatusDTO switch
        {
            OrderStatusDTO.Processing => OrderStatus.Processing,
            OrderStatusDTO.Delivering => OrderStatus.Delivering,
            OrderStatusDTO.Delivered => OrderStatus.Delivered,
            OrderStatusDTO.Completed => OrderStatus.Completed,
            _ => throw new ArgumentOutOfRangeException(nameof(orderStatusDTO), orderStatusDTO, null)
        };
    }
}