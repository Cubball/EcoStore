namespace EcoStore.BLL.DTO;

public class UpdateOrderTrackingNumberDTO
{
    public int Id { get; set; }

    public string TrackingNumber { get; set; } = default!;
}