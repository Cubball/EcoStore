namespace EcoStore.BLL.DTO;

public class OrderedProductDTO
{
    public ProductDTO Product { get; set; } = default!;

    public int Quantity { get; set; }

    public decimal ProductPrice { get; set; }
}