namespace EcoStore.BLL.DTO;

public class ProductDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }

    public string ImageUrl { get; set; } = default!;

    public int Stock { get; set; }

    public BrandDTO Brand { get; set; } = default!;

    public CategoryDTO Category { get; set; } = default!;
}