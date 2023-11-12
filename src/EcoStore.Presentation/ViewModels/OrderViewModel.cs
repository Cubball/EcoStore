namespace EcoStore.Presentation.ViewModels;

public class OrderViewModel
{
    public int Id { get; set; }

    public AppUserViewModel User { get; set; } = default!;

    public List<OrderedProductViewModel> OrderedProducts { get; set; } = default!;

    public decimal TotalPrice => OrderedProducts.Sum(op => op.TotalPrice);

    public DateTime OrderDate { get; set; }

    public OrderStatusViewModel OrderStatus { get; set; }

    public DateTime StatusChangedDate { get; set; }

    public PaymentMethodViewModel PaymentMethod { get; set; }

    public PaymentViewModel? Payment { get; set; }

    public string ShippingAddress { get; set; } = default!;

    public ShippingMethodViewModel ShippingMethod { get; set; }

    public string? TrackingNumber { get; set; }
}