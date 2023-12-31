namespace EcoStore.DAL.Entities;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public ICollection<Product> Products { get; } = new List<Product>();
}