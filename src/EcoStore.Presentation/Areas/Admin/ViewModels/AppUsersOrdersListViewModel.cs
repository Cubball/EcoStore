using EcoStore.Presentation.ViewModels;

namespace EcoStore.Presentation.Areas.Admin.ViewModels;

public class AppUsersOrdersListViewModel
{
    public List<OrderViewModel> Orders { get; set; } = default!;

    public AppUserViewModel User { get; set; } = default!;

    public PageInfoViewModel PageInfo { get; set; } = default!;
}