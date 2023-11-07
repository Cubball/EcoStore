using EcoStore.BLL.DTO;

namespace EcoStore.BLL.Services.Interfaces;

public interface IOrderService
{
    Task<int> CreateOrderAsync(CreateOrderDTO orderDTO);

    Task<OrderDTO> GetOrderAsync(int id);

    Task<IEnumerable<OrderDTO>> GetOrdersAsync(string? userId = null, int? pageNumber = null, int? pageSize = null);

    Task<int> GetOrderCountAsync(string? userId = null, DateTime? startDate = null, DateTime? endDate = null);

    Task UpdateOrderStatusAsync(UpdateOrderStatusDTO orderDTO);

    Task UpdateOrderTrackingNumberAsync(UpdateOrderTrackingNumberDTO orderDTO);

    Task DeleteOrderAsync(int id);

    Task CancelOrderByUserAsync(CancelOrderByUserDTO orderDTO);

    Task CancelOrderByAdminAsync(CancelOrderByAdminDTO orderDTO);
}