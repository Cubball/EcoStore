using EcoStore.DAL.Entities;

namespace EcoStore.DAL.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrdersAsync(int? skip = null, int? count = null, Predicate<Order>? predicate = null);

    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId, int? skip = null, int? count = null);

    Task<Order> GetOrderByIdAsync(int id);

    Task<int> AddOrderAsync(Order order);

    Task UpdateOrderAsync(int id, Action<Order> updateAction);

    Task DeleteOrderAsync(int id);
}