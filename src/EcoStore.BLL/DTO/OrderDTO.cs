namespace EcoStore.BLL.DTO;

// TODO: Use enums here?
public class OrderDTO
{
    public int Id { get; set; }

    public AppUserDTO? User { get; set; }

    public IEnumerable<OrderedProductDTO> OrderedProducts { get; set; } = default!;

    public DateTime OrderDate { get; set; }

    public string OrderStatus { get; set; } = default!;

    public DateTime StatusChangedDate { get; set; }

    public string PaymentMethod { get; set; } = default!;

    public PaymentDTO? Payment { get; set; }

    public string ShippingAddress { get; set; } = default!;

    public string ShippingMethod { get; set; } = default!;

    public string? TrackingNumber { get; set; }
}