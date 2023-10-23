using Microsoft.EntityFrameworkCore;

using Project.Data;
using Project.Models;
using Project.Repositories.Exceptions;
using Project.Repositories.Interfaces;

namespace Project.Repositories;

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
            _context.OrderedProducts.AddRange(order.OrderedProducts);
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
            _context.OrderedProducts.RemoveRange(order.OrderedProducts);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося видалити замовлення", e);
        }
    }

    public async Task<Order> GetOrderByIdAsync(int id, bool includeUser = false)
    {
        return await _context.Orders
            .Include(o => includeUser ? o.User : null)
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

    private void UpdateOrderProperties(Order orderFromDb, Order newOrder)
    {
        // TODO check if this accually works
        var orderedProductsToDelete = orderFromDb.OrderedProducts
            .ExceptBy(newOrder.OrderedProducts
                    .Select(op => op.ProductId), op => op.ProductId);
        _context.OrderedProducts.RemoveRange(orderedProductsToDelete);
        _context.OrderedProducts.UpdateRange(newOrder.OrderedProducts);

        orderFromDb.UserId = newOrder.UserId;
        orderFromDb.OrderDate = newOrder.OrderDate;
        orderFromDb.OrderStatus = newOrder.OrderStatus;
        orderFromDb.StatusChangedDate = newOrder.StatusChangedDate;
        orderFromDb.PaymentMethod = newOrder.PaymentMethod;
        orderFromDb.IsPaid = newOrder.IsPaid;
        orderFromDb.ShippingAddress = newOrder.ShippingAddress;
        orderFromDb.ShippingMethod = newOrder.ShippingMethod;
        orderFromDb.TrackingNumber = newOrder.TrackingNumber;
    }
}