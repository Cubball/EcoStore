namespace EcoStore.BLL.DTO;

public class ProductDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }

    public string ImageName { get; set; } = default!;

    public int Stock { get; set; }

    public BrandDTO? Brand { get; set; }

    public CategoryDTO? Category { get; set; }
}