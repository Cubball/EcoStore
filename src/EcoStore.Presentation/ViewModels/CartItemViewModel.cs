namespace EcoStore.Presentation.ViewModels;

public class CartItemViewModel
{
    public ProductViewModel Product { get; set; } = default!;

    public int Quantity { get; set; }
}