using EcoStore.Presentation.ViewModels;

namespace EcoStore.Presentation.Areas.Admin.ViewModels;

public class AppUsersListViewModel
{
    public PageInfoViewModel PageInfo { get; set; } = default!;

    public IEnumerable<AppUserViewModel> Users { get; set; } = default!;

    public string? Search { get; set; }
}