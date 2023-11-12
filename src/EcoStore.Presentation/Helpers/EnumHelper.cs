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
}