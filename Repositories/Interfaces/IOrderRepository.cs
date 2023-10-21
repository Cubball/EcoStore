using Project.Models;

namespace Project.Repositories.Interfaces;

public interface IOrderRepository : IDisposable
{
    Task<IEnumerable<Order>> GetOrdersAsync();

    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);

    Task<Order> GetOrderByIdAsync(int id, bool includeUser = false);

    Task<int> AddOrderAsync(Order order);

    Task UpdateOrderAsync(Order order);

    Task DeleteOrderAsync(int id);
}