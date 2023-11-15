using EcoStore.BLL.DTO;
using EcoStore.Presentation.Areas.Admin.ViewModels;

namespace EcoStore.Presentation.Areas.Admin.Mapping;

public static class EnumsMappingExtensions
{
    public static NewOrderStatusDTO ToDTO(this NewOrderStatusViewModel newOrderStatusViewModel)
    {
        return newOrderStatusViewModel switch
        {
            NewOrderStatusViewModel.Processing => NewOrderStatusDTO.Processing,
            NewOrderStatusViewModel.Delivering => NewOrderStatusDTO.Delivering,
            NewOrderStatusViewModel.Delivered => NewOrderStatusDTO.Delivered,
            NewOrderStatusViewModel.Completed => NewOrderStatusDTO.Completed,
            _ => throw new ArgumentOutOfRangeException(nameof(newOrderStatusViewModel), newOrderStatusViewModel, null)
        };
    }

    public static (SortSalesByDTO SortBy, bool Descending) ToDTO(this SortSalesByViewModel sortSalesByViewModel)
    {
        return sortSalesByViewModel switch
        {
            SortSalesByViewModel.Name => (SortSalesByDTO.Name, false),
            SortSalesByViewModel.NameDesc => (SortSalesByDTO.Name, true),
            SortSalesByViewModel.Revenue => (SortSalesByDTO.Revenue, false),
            SortSalesByViewModel.RevenueDesc => (SortSalesByDTO.Revenue, true),
            SortSalesByViewModel.NumberSold => (SortSalesByDTO.NumberSold, false),
            SortSalesByViewModel.NumberSoldDesc => (SortSalesByDTO.NumberSold, true),
            SortSalesByViewModel.DateCreated => (SortSalesByDTO.DateCreated, false),
            SortSalesByViewModel.DateCreatedDesc => (SortSalesByDTO.DateCreated, true),
            _ => throw new ArgumentOutOfRangeException(nameof(sortSalesByViewModel), sortSalesByViewModel, null)
        };
    }

    public static (SortProductsInReportByDTO SortBy, bool Descending) ToDTO(this SortProductsInReportByViewModel sortProductsViewModel)
    {
        return sortProductsViewModel switch
        {
            SortProductsInReportByViewModel.Name => (SortProductsInReportByDTO.Name, false),
            SortProductsInReportByViewModel.NameDesc => (SortProductsInReportByDTO.Name, true),
            SortProductsInReportByViewModel.Stock => (SortProductsInReportByDTO.Stock, false),
            SortProductsInReportByViewModel.StockDesc => (SortProductsInReportByDTO.Stock, true),
            SortProductsInReportByViewModel.Price => (SortProductsInReportByDTO.Price, false),
            SortProductsInReportByViewModel.PriceDesc => (SortProductsInReportByDTO.Price, true),
            SortProductsInReportByViewModel.DateCreated => (SortProductsInReportByDTO.DateCreated, false),
            SortProductsInReportByViewModel.DateCreatedDesc => (SortProductsInReportByDTO.DateCreated, true),
            _ => throw new ArgumentOutOfRangeException(nameof(sortProductsViewModel), sortProductsViewModel, null)
        };
    }
}