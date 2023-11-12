using EcoStore.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcoStore.Presentation.Helpers;

public static class EnumHelper
{
    public static IEnumerable<SelectListItem> SortByEnumSelectList { get; } = new[]
    {
        new SelectListItem("Назва: від А до Я", "Name"),
        new SelectListItem("Назва: від Я до А", "NameDesc"),
        new SelectListItem("Ціна: спочатку найдешевші", "Price"),
        new SelectListItem("Ціна: спочатку найдорожчі", "PriceDesc"),
        new SelectListItem("Спочатку старіші", "DateCreated"),
        new SelectListItem("Спочатку новіші", "DateCreatedDesc"),
    };

    public static IEnumerable<SelectListItem> PaymentMethodSelectList { get; } = new[]
    {
        new SelectListItem("При отриманні", "Cash"),
        new SelectListItem("Карткою", "Card"),
    };

    public static IEnumerable<SelectListItem> ShippingMethodSelectList { get; } = new[]
    {
        new SelectListItem("Нова пошта", "NovaPoshta"),
        new SelectListItem("Укрпошта", "UkrPoshta"),
    };

    public static string GetOrderStatusName(OrderStatusViewModel orderStatus)
    {
        return orderStatus switch
        {
            OrderStatusViewModel.New => "Нове",
            OrderStatusViewModel.Processing => "В обробці",
            OrderStatusViewModel.Delivering => "Відправлене",
            OrderStatusViewModel.Delivered => "Доставлене",
            OrderStatusViewModel.Completed => "Завершене",
            OrderStatusViewModel.CancelledByUser => "Скасоване Вами",
            OrderStatusViewModel.CancelledByAdmin => "Скасоване адміністратором",
            _ => throw new ArgumentOutOfRangeException(nameof(orderStatus), orderStatus, null)
        };
    }

    public static string GetPaymentMethodName(PaymentMethodViewModel paymentMethod)
    {
        return paymentMethod switch
        {
            PaymentMethodViewModel.Cash => "При отриманні",
            PaymentMethodViewModel.Card => "Карткою",
            _ => throw new ArgumentOutOfRangeException(nameof(paymentMethod), paymentMethod, null)
        };
    }

    public static string GetShippingMethodName(ShippingMethodViewModel shippingMethod)
    {
        return shippingMethod switch
        {
            ShippingMethodViewModel.UkrPoshta => "Укрпошта",
            ShippingMethodViewModel.NovaPoshta => "Нова пошта",
            _ => throw new ArgumentOutOfRangeException(nameof(shippingMethod), shippingMethod, null)
        };
    }
}