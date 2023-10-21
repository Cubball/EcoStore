namespace Project.Models;

public class OrderedProduct
{
    public int ProductId { get; set; }

    public Product Product { get; set; } = default!;

    public int OrderId { get; set; }

    public Order Order { get; set; } = default!;

    public int Quantity { get; set; }

    public decimal ProductPrice { get; set; }

    public decimal TotalPrice => Quantity * ProductPrice;
}