namespace EcoStore.Presentation.ViewModels;

public class CategoryDetailsViewModel
{
    public CategoryViewModel Category { get; set; } = default!;

    public IEnumerable<ProductViewModel> Products { get; set; } = default!;
}