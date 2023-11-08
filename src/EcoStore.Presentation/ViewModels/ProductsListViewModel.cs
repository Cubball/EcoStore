namespace EcoStore.Presentation.ViewModels;

public class ProductsListViewModel
{
    public PageInfoViewModel PageInfo { get; set; } = default!;

    public IEnumerable<ProductViewModel> Products { get; set; } = default!;
}