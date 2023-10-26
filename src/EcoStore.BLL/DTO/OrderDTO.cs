namespace EcoStore.BLL.DTO;

public class OrderDTO
{
    public int Id { get; set; }

    // Change to DTO?
    public string UserId { get; set; } = default!;

    public DateTime OrderDate { get; set; }

    public string OrderStatus { get; set; } = default!;

    public DateTime StatusChangedDate { get; set; }

    public string PaymentMethod { get; set; } = default!;

    // Change to DTO?
    public string? PaymentId { get; set; }
}