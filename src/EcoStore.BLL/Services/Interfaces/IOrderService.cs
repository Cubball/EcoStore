using EcoStore.BLL.DTO;

namespace EcoStore.BLL.Services.Interfaces;

public interface IOrderService
{
    Task<int> CreateOrderAsync(CreateOrderDTO orderDTO);

    Task<OrderDTO> GetOrderAsync(int id);

    Task<IEnumerable<OrderDTO>> GetOrdersAsync();

    Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(string userId);

    Task UpdateOrderStatusAsync(UpdateOrderStatusDTO orderDTO);

    Task UpdateOrderTrackingNumberAsync(UpdateOrderTrackingNumberDTO orderDTO);

    Task DeleteOrderAsync(int id);

    Task CancelOrderAsUserAsync(int id);

    Task CancelOrderAsAdminAsync(int id);
}