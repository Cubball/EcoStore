namespace EcoStore.Presentation.ViewModels;

public class OrderedProductViewModel
{
    public ProductViewModel? Product { get; set; } = default!;

    public int Quantity { get; set; }

    public decimal ProductPrice { get; set; }

    public decimal TotalPrice => Quantity * ProductPrice;
}