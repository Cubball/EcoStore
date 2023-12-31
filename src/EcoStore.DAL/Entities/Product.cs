namespace EcoStore.DAL.Entities;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }

    public string ImageName { get; set; } = default!;

    public int Stock { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; } = default!;

    public int BrandId { get; set; }

    public Brand Brand { get; set; } = default!;
}