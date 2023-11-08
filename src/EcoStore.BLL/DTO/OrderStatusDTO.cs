namespace EcoStore.BLL.DTO;

public enum OrderStatusDTO
{
    New,
    Processing,
    Delivering,
    Delivered,
    Completed,
    CancelledByUser,
    CancelledByAdmin,
}