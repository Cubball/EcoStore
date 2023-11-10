namespace EcoStore.Presentation.ViewModels;

public class CreateOrderViewModel
{
    public string? StripeToken { get; set; }

    public PaymentMethodViewModel PaymentMethod { get; set; }

    public string ShippingAddress { get; set; } = default!;

    public ShippingMethodViewModel ShippingMethod { get; set; }

    public CartViewModel Cart { get; set; } = default!;
}