using EcoStore.Presentation.Areas.Admin.ViewModels;
using EcoStore.Presentation.ViewModels;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcoStore.Presentation.Helpers;

public static class EnumHelper
{
    public static IEnumerable<SelectListItem> SortByEnumSelectList { get; } = new[]
    {
        new SelectListItem("Назва: від А до Я", SortProductsByViewModel.Name.ToString()),
        new SelectListItem("Назва: від Я до А", SortProductsByViewModel.NameDesc.ToString()),
        new SelectListItem("Ціна: спочатку найдешевші", SortProductsByViewModel.Price.ToString()),
        new SelectListItem("Ціна: спочатку найдорожчі", SortProductsByViewModel.PriceDesc.ToString()),
        new SelectListItem("Спочатку старіші", SortProductsByViewModel.DateCreated.ToString()),
        new SelectListItem("Спочатку новіші", SortProductsByViewModel.DateCreatedDesc.ToString()),
    };

    public static IEnumerable<SelectListItem> SortProductsInReportBySelectList { get; } = new[]
    {
        new SelectListItem("Назва: від А до Я", SortProductsInReportByViewModel.Name.ToString()),
        new SelectListItem("Назва: від Я до А", SortProductsInReportByViewModel.NameDesc.ToString()),
        new SelectListItem("Наявіність: спочатку ті, яких менше", SortProductsInReportByViewModel.Stock.ToString()),
        new SelectListItem("Наявіність: спочатку ті, яких більше", SortProductsInReportByViewModel.StockDesc.ToString()),
        new SelectListItem("Ціна: спочатку найдешевші", SortProductsInReportByViewModel.Price.ToString()),
        new SelectListItem("Ціна: спочатку найдорожчі", SortProductsInReportByViewModel.PriceDesc.ToString()),
        new SelectListItem("Спочатку старіші", SortProductsInReportByViewModel.DateCreated.ToString()),
        new SelectListItem("Спочатку новіші", SortProductsInReportByViewModel.DateCreatedDesc.ToString()),
    };

    public static IEnumerable<SelectListItem> SortSalesBySelectList { get; } = new[]
    {
        new SelectListItem("Назва: від А до Я", SortSalesByViewModel.Name.ToString()),
        new SelectListItem("Назва: від Я до А", SortSalesByViewModel.NameDesc.ToString()),
        new SelectListItem("Кількість продано: спочатку ті, яких менше", SortSalesByViewModel.NumberSold.ToString()),
        new SelectListItem("Кількість продано: спочатку ті, яких більше", SortSalesByViewModel.NumberSoldDesc.ToString()),
        new SelectListItem("Сума продажів: спочатку ті, яких менше", SortSalesByViewModel.Revenue.ToString()),
        new SelectListItem("Сума продажів: спочатку ті, яких більше", SortSalesByViewModel.RevenueDesc.ToString()),
        new SelectListItem("Спочатку старіші", SortSalesByViewModel.DateCreated.ToString()),
        new SelectListItem("Спочатку новіші", SortSalesByViewModel.DateCreatedDesc.ToString()),
    };

    public static IEnumerable<SelectListItem> PaymentMethodSelectList { get; } = new[]
    {
        new SelectListItem("При отриманні", PaymentMethodViewModel.Cash.ToString()),
        new SelectListItem("Карткою", PaymentMethodViewModel.Card.ToString()),
    };

    public static IEnumerable<SelectListItem> ShippingMethodSelectList { get; } = new[]
    {
        new SelectListItem("Нова пошта", ShippingMethodViewModel.NovaPoshta.ToString()),
        new SelectListItem("Укрпошта", ShippingMethodViewModel.UkrPoshta.ToString()),
    };

    public static IEnumerable<SelectListItem> NewOrderStatusSelectList { get; } = new[]
    {
        new SelectListItem("В обробці", NewOrderStatusViewModel.Processing.ToString()),
        new SelectListItem("Відправлене", NewOrderStatusViewModel.Delivering.ToString()),
        new SelectListItem("Доставлене", NewOrderStatusViewModel.Delivered.ToString()),
        new SelectListItem("Завершене", NewOrderStatusViewModel.Completed.ToString()),
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
            OrderStatusViewModel.CancelledByUser => "Скасоване покупцем",
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

    public static string GetRoleName(RoleViewModel role)
    {
        return role switch
        {
            RoleViewModel.User => "Користувач",
            RoleViewModel.Admin => "Адміністратор",
            _ => throw new ArgumentOutOfRangeException(nameof(role)),
        };
    }
}