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

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderedProducts)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderedProducts)
                .ThenInclude(op => op.Product)
            .ToListAsync();
    }

    public async Task UpdateOrderAsync(Order order)
    {
        var retrievedOrder = await GetOrderByIdAsync(order.Id);
        try
        {
            UpdateOrderProperties(retrievedOrder, order);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося оновити замовлення", e);
        }
    }

    private static void UpdateOrderProperties(Order orderFromDb, Order newOrder)
    {
        orderFromDb.OrderedProducts.Clear();
        foreach (var orderedProduct in newOrder.OrderedProducts)
        {
            orderFromDb.OrderedProducts.Add(orderedProduct);
        }

        orderFromDb.UserId = newOrder.UserId;
        orderFromDb.OrderDate = newOrder.OrderDate;
        orderFromDb.OrderStatus = newOrder.OrderStatus;
        orderFromDb.StatusChangedDate = newOrder.StatusChangedDate;
        orderFromDb.PaymentMethod = newOrder.PaymentMethod;
        orderFromDb.PaymentId = newOrder.PaymentId;
        orderFromDb.ShippingAddress = newOrder.ShippingAddress;
        orderFromDb.ShippingMethod = newOrder.ShippingMethod;
        orderFromDb.TrackingNumber = newOrder.TrackingNumber;
    }
}