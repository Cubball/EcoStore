namespace EcoStore.Presentation.Areas.Admin.ViewModels;

public class UpdateProductViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }

    public IFormFile? Image { get; set; }

    public int Stock { get; set; }

    public int CategoryId { get; set; }

    public int BrandId { get; set; }
}