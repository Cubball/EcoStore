using EcoStore.DAL.Entities;

namespace EcoStore.DAL.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrdersAsync();

    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);

    Task<Order> GetOrderByIdAsync(int id);

    Task<int> AddOrderAsync(Order order);

    Task UpdateOrderAsync(int id, Action<Order> updateAction);

    Task DeleteOrderAsync(int id);
}