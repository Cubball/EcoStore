using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcoStore.Presentation.ViewModels;

public class ProductsListViewModel
{
    public PageInfoViewModel PageInfo { get; set; } = default!;

    public IEnumerable<ProductViewModel> Products { get; set; } = default!;

    public ProductFilterViewModel Filter { get; set; } = default!;

    public IEnumerable<SelectListItem> Brands { get; set; } = default!;

    public IEnumerable<SelectListItem> Categories { get; set; } = default!;
}