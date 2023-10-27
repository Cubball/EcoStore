namespace EcoStore.BLL.DTO;

public class CreateOrderDTO
{
    public string UserId { get; set; } = default!;

    public string? StripeToken { get; set; }

    public PaymentMethodDTO PaymentMethod { get; set; }

    public string ShippingAddress { get; set; } = default!;

    public ShippingMethodDTO ShippingMethod { get; set; }

    public IEnumerable<CreateOrderedProductDTO> OrderedProducts { get; set; } = default!;
}