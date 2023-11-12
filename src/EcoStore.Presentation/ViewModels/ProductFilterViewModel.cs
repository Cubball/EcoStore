namespace EcoStore.Presentation.ViewModels;

public class ProductFilterViewModel
{
    public int[] Categories { get; set; } = default!;

    public int[] Brands { get; set; } = default!;

    public SortProductsByViewModel SortBy { get; set; }

    public int? MinPrice { get; set; }

    public int? MaxPrice { get; set; }

    public string? Search { get; set; }
}