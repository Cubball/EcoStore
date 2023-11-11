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

    public static SortSalesByDTO ToDTO(this SortSalesByViewModel sortSalesByViewModel)
    {
        return sortSalesByViewModel switch
        {
            SortSalesByViewModel.Name => SortSalesByDTO.Name,
            SortSalesByViewModel.Revenue => SortSalesByDTO.Revenue,
            SortSalesByViewModel.NumberSold => SortSalesByDTO.NumberSold,
            SortSalesByViewModel.DateCreated => SortSalesByDTO.DateCreated,
            _ => throw new ArgumentOutOfRangeException(nameof(sortSalesByViewModel), sortSalesByViewModel, null)
        };
    }

    public static SortProductsInReportByDTO ToDTO(this SortProductsInReportByViewModel sortProductsViewModel)
    {
        return sortProductsViewModel switch
        {
            SortProductsInReportByViewModel.Name => SortProductsInReportByDTO.Name,
            SortProductsInReportByViewModel.Stock => SortProductsInReportByDTO.Stock,
            SortProductsInReportByViewModel.Price => SortProductsInReportByDTO.Price,
            SortProductsInReportByViewModel.DateCreated => SortProductsInReportByDTO.DateCreated,
            _ => throw new ArgumentOutOfRangeException(nameof(sortProductsViewModel), sortProductsViewModel, null)
        };
    }
}