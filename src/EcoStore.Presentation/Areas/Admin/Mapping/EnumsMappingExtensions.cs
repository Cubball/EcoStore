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
}