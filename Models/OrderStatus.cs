namespace Project.Models;

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