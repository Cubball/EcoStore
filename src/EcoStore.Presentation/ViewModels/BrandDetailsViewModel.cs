namespace EcoStore.Presentation.ViewModels;

public class BrandDetailsViewModel
{
    public BrandViewModel Brand { get; set; } = default!;

    public IEnumerable<ProductViewModel> Products { get; set; } = default!;
}