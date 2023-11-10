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

    public static PaymentMethodDTO ToDTO(this PaymentMethodViewModel paymentMethodViewModel)
    {
        return paymentMethodViewModel switch
        {
            PaymentMethodViewModel.Cash => PaymentMethodDTO.Cash,
            PaymentMethodViewModel.Card => PaymentMethodDTO.Card,
            _ => throw new ArgumentOutOfRangeException(nameof(paymentMethodViewModel), paymentMethodViewModel, null),
        };
    }

    public static ShippingMethodDTO ToDTO(this ShippingMethodViewModel shippingMethodViewModel)
    {
        return shippingMethodViewModel switch
        {
            ShippingMethodViewModel.NovaPoshta => ShippingMethodDTO.NovaPoshta,
            ShippingMethodViewModel.UkrPoshta => ShippingMethodDTO.UkrPoshta,
            _ => throw new ArgumentOutOfRangeException(nameof(shippingMethodViewModel), shippingMethodViewModel, null),
        };
    }

    public static SortByDTO ToDTO(this SortProductsByViewModel sortByViewModel)
    {
        return sortByViewModel switch
        {
            SortProductsByViewModel.Name => SortByDTO.Name,
            SortProductsByViewModel.Price => SortByDTO.Price,
            SortProductsByViewModel.DateCreated => SortByDTO.DateCreated,
            _ => throw new ArgumentOutOfRangeException(nameof(sortByViewModel), sortByViewModel, null),
        };
    }
}