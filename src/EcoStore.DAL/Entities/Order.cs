namespace EcoStore.DAL.Entities;

public class Order
{
    public int Id { get; set; }

    public AppUser User { get; set; } = default!;

    public string UserId { get; set; } = default!;

    public ICollection<OrderedProduct> OrderedProducts { get; } = new List<OrderedProduct>();

    public decimal TotalPrice => OrderedProducts.Sum(op => op.TotalPrice);

    public DateTime OrderDate { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public DateTime StatusChangedDate { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public bool IsPaid { get; set; }

    public string ShippingAddress { get; set; } = default!;

    public ShippingMethod ShippingMethod { get; set; }

    public string? TrackingNumber { get; set; }
}