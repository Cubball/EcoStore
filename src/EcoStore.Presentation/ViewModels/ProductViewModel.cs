namespace EcoStore.Presentation.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }

    public string ImagePath { get; set; } = default!;

    public int Stock { get; set; }

    public CategoryViewModel? Category { get; set; }

    public BrandViewModel? Brand { get; set; }
}