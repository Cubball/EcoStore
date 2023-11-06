namespace EcoStore.BLL.DTO;

public class CreateProductDTO
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }

    public string ImageExtension { get; set; } = default!;

    public Stream ImageStream { get; set; } = default!;

    public int Stock { get; set; }

    public int BrandId { get; set; }

    public int CategoryId { get; set; }
}