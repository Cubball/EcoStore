using EcoStore.BLL.DTO;

namespace EcoStore.BLL.Services.Interfaces;

public interface IOrderService
{
    Task<int> CreateOrderAsync(CreateOrderDTO orderDTO);

    Task<OrderDTO> GetOrderAsync(int id);

    Task<IEnumerable<OrderDTO>> GetOrdersAsync(int? pageNumber = null, int? pageSize = null);

    Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(string userId, int? pageNumber = null, int? pageSize = null);

    Task UpdateOrderStatusAsync(UpdateOrderStatusDTO orderDTO);

    Task UpdateOrderTrackingNumberAsync(UpdateOrderTrackingNumberDTO orderDTO);

    Task DeleteOrderAsync(int id);

    Task CancelOrderByUserAsync(CancelOrderByUserDTO orderDTO);

    Task CancelOrderByAdminAsync(CancelOrderByAdminDTO orderDTO);
}