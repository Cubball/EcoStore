namespace EcoStore.BLL.DTO;

public class UpdateOrderStatusDTO
{
    public int Id { get; set; }

    public NewOrderStatusDTO OrderStatus { get; set; }
}