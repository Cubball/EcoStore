using System.Linq.Expressions;

using EcoStore.DAL.Entities;

namespace EcoStore.DAL.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrdersAsync(
            int? skip = null,
            int? count = null,
            Expression<Func<Order, bool>>? predicate = null,
            Expression<Func<Order, object>>? orderBy = null,
            bool descending = false);

    Task<int> GetOrdersCountAsync(Expression<Func<Order, bool>>? predicate = null);

    Task<Order> GetOrderByIdAsync(int id);

    Task<int> AddOrderAsync(Order order);

    Task UpdateOrderAsync(int id, Action<Order> updateAction);

    Task DeleteOrderAsync(int id);
}