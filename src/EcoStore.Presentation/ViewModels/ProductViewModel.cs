namespace EcoStore.Presentation.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }

    public string ImagePath { get; set; } = default!;

    public int Stock { get; set; }

    public int? CategoryId { get; set; }

    public string? Category { get; set; }

    public int? BrandId { get; set; }

    public string? Brand { get; set; }
}