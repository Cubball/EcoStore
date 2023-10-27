using EcoStore.DAL.EF;
using EcoStore.DAL.Entities;
using EcoStore.DAL.Repositories.Exceptions;
using EcoStore.DAL.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace EcoStore.DAL.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddProductAsync(Product product)
    {
        try
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося додати товар", e);
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await GetProductByIdAsync(id);
        try
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося видалити товар", e);
        }
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new EntityNotFoundException($"Товар з Id {id} не знайдено");
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(
            int? skip = null, int? count = null,
            Predicate<Product>? predicate = null)
    {
        var products = _context.Products.AsQueryable();
        if (skip is not null)
        {
            products = products.Skip(skip.Value);
        }

        if (count is not null)
        {
            products = products.Take(count.Value);
        }

        if (predicate is not null)
        {
            products = products.Where(p => predicate(p));
        }

        return await products.ToListAsync();
    }

    public async Task UpdateProductAsync(int id, Action<Product> updateAction)
    {
        var product = await GetProductByIdAsync(id);
        updateAction(product);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося оновити товар", e);
        }
    }

    public async Task<bool> ProductExistsAsync(int id)
    {
        return (await _context.Products.FindAsync(id)) is not null;
    }

    public async Task<bool> ProductExistsAsync(string name)
    {
        return await _context.Products.AnyAsync(p => p.Name == name);
    }
}