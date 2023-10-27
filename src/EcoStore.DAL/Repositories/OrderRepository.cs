using System.Linq.Expressions;

using EcoStore.DAL.EF;
using EcoStore.DAL.Entities;
using EcoStore.DAL.Repositories.Exceptions;
using EcoStore.DAL.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace EcoStore.DAL.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddOrderAsync(Order order)
    {
        try
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.Id;
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося додати замовлення", e);
        }
    }

    public async Task DeleteOrderAsync(int id)
    {
        var order = await GetOrderByIdAsync(id);
        try
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося видалити замовлення", e);
        }
    }

    public async Task<Order> GetOrderByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Payment)
            .Include(o => o.OrderedProducts)
                .ThenInclude(op => op.Product)
            .FirstOrDefaultAsync(o => o.Id == id)
            ?? throw new EntityNotFoundException($"Замовлення з Id {id} не знайдено");
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync(
            int? skip = null,
            int? count = null,
            Expression<Func<Order, bool>>? predicate = null,
            Expression<Func<Order, object>>? orderBy = null,
            bool descending = false)
    {
        var orders = _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderedProducts)
            .AsQueryable();
        if (predicate is not null)
        {
            orders = orders.Where(predicate);
        }

        if (orderBy is not null)
        {
            orders = descending
                ? orders.OrderByDescending(orderBy)
                : orders.OrderBy(orderBy);
        }

        if (skip is not null)
        {
            orders = orders.Skip(skip.Value);
        }

        if (count is not null)
        {
            orders = orders.Take(count.Value);
        }

        return await orders.ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId,
            int? skip = null,
            int? count = null,
            Expression<Func<Order, object>>? orderBy = null,
            bool descending = false)
    {
        var orders = _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderedProducts)
                .ThenInclude(op => op.Product)
            .Include(o => o.Payment)
            .AsQueryable();
        if (orderBy is not null)
        {
            orders = descending
                ? orders.OrderByDescending(orderBy)
                : orders.OrderBy(orderBy);
        }

        if (skip is not null)
        {
            orders = orders.Skip(skip.Value);
        }

        if (count is not null)
        {
            orders = orders.Take(count.Value);
        }

        return await orders.ToListAsync();
    }

    public async Task UpdateOrderAsync(int id, Action<Order> updateAction)
    {
        var order = await GetOrderByIdAsync(id);
        updateAction(order);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося оновити замовлення", e);
        }
    }
}