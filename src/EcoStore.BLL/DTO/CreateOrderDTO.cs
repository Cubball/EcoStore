namespace EcoStore.BLL.DTO;

public class CreateOrderDTO
{
    public string UserId { get; set; } = default!;

    public string? StripeToken { get; set; }

    public string PaymentMethod { get; set; } = default!;

    public string ShippingAddress { get; set; } = default!;

    public string ShippingMethod { get; set; } = default!;

    public IEnumerable<CreateOrderedProductDTO> OrderedProducts { get; set; } = default!;
}