using EcoStore.Presentation.ViewModels;

namespace EcoStore.Presentation.Areas.Admin.ViewModels;

public class BrandsListViewModel
{
    public PageInfoViewModel PageInfo { get; set; } = default!;

    public IEnumerable<BrandViewModel> Brands { get; set; } = default!;
}