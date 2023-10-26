namespace EcoStore.BLL.DTO;

public class UpdateOrderStatusDTO
{
    public int Id { get; set; }

    public string OrderStatus { get; set; } = default!;
}