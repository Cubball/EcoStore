using EcoStore.BLL.DTO;
using EcoStore.Presentation.ViewModels;

namespace EcoStore.Presentation.Mapping;

public static class EnumsMappingExtensions
{
    public static RoleViewModel ToViewModel(this RoleDTO roleDTO)
    {
        return roleDTO switch
        {
            RoleDTO.Admin => RoleViewModel.Admin,
            RoleDTO.User => RoleViewModel.User,
            _ => throw new ArgumentOutOfRangeException(nameof(roleDTO), roleDTO, null),
        };
    }

    public static OrderStatusViewModel ToViewModel(this OrderStatusDTO orderStatusDTO)
    {
        return orderStatusDTO switch
        {
            OrderStatusDTO.New => OrderStatusViewModel.New,
            OrderStatusDTO.Processing => OrderStatusViewModel.Processing,
            OrderStatusDTO.Delivering => OrderStatusViewModel.Delivering,
            OrderStatusDTO.Delivered => OrderStatusViewModel.Delivered,
            OrderStatusDTO.Completed => OrderStatusViewModel.Completed,
            OrderStatusDTO.CancelledByUser => OrderStatusViewModel.CancelledByUser,
            OrderStatusDTO.CancelledByAdmin => OrderStatusViewModel.CancelledByAdmin,
            _ => throw new ArgumentOutOfRangeException(nameof(orderStatusDTO), orderStatusDTO, null),
        };
    }

    public static PaymentMethodViewModel ToViewModel(this PaymentMethodDTO paymentMethodDTO)
    {
        return paymentMethodDTO switch
        {
            PaymentMethodDTO.Cash => PaymentMethodViewModel.Cash,
            PaymentMethodDTO.Card => PaymentMethodViewModel.Card,
            _ => throw new ArgumentOutOfRangeException(nameof(paymentMethodDTO), paymentMethodDTO, null),
        };
    }

    public static ShippingMethodViewModel ToViewModel(this ShippingMethodDTO shippingMethodDTO)
    {
        return shippingMethodDTO switch
        {
            ShippingMethodDTO.NovaPoshta => ShippingMethodViewModel.NovaPoshta,
            ShippingMethodDTO.UkrPoshta => ShippingMethodViewModel.UkrPoshta,
            _ => throw new ArgumentOutOfRangeException(nameof(shippingMethodDTO), shippingMethodDTO, null),
        };
    }
}