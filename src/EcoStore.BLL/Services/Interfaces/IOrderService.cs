using EcoStore.BLL.DTO;

namespace EcoStore.BLL.Services.Interfaces;

public interface IOrderService
{
    Task<int> CreateOrderAsync(CreateOrderDTO orderDTO);
}