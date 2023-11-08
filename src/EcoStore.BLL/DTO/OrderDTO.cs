namespace EcoStore.BLL.DTO;

public class OrderDTO
{
    public int Id { get; set; }

    public AppUserDTO? User { get; set; }

    public IEnumerable<OrderedProductDTO> OrderedProducts { get; set; } = default!;

    public DateTime OrderDate { get; set; }

    public OrderStatusDTO OrderStatus { get; set; } = default!;

    public DateTime StatusChangedDate { get; set; }

    public PaymentMethodDTO PaymentMethod { get; set; } = default!;

    public PaymentDTO? Payment { get; set; }

    public string ShippingAddress { get; set; } = default!;

    public ShippingMethodDTO ShippingMethod { get; set; } = default!;

    public string? TrackingNumber { get; set; }
}