namespace EcoStore.DAL.Entities;

public enum OrderStatus
{
    New,
    Processing,
    Delivering,
    Delivered,
    Completed,
    CancelledByUser,
    CancelledByAdmin,
}