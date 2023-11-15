using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcoStore.Presentation.Areas.Admin.ViewModels;

public class CreateProductViewModel
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }

    public IFormFile Image { get; set; } = default!;

    public int Stock { get; set; }

    public int CategoryId { get; set; }

    public int BrandId { get; set; }

    public List<SelectListItem> Categories { get; set; } = default!;

    public List<SelectListItem> Brands { get; set; } = default!;
}