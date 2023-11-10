using EcoStore.Presentation.ViewModels;

namespace EcoStore.Presentation.Areas.Admin.ViewModels;

public class CategoriesListViewModel
{
    public PageInfoViewModel PageInfo { get; set; } = default!;

    public IEnumerable<CategoryViewModel> Categories { get; set; } = default!;
}