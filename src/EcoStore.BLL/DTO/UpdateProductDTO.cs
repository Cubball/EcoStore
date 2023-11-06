namespace EcoStore.BLL.DTO;

public class UpdateProductDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }

    public string? ImageExtension { get; set; }

    public Stream? ImageStream { get; set; }

    public int Stock { get; set; }

    public int BrandId { get; set; }

    public int CategoryId { get; set; }
}