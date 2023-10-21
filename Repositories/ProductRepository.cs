using Microsoft.EntityFrameworkCore;

using Project.Data;
using Project.Models;
using Project.Repositories.Exceptions;
using Project.Repositories.Interfaces;

namespace Project.Repositories;

public class ProductRepository : IProductRepository
{
    private bool _disposed;
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

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByBrandIdAsync(int brandId)
    {
        return await _context.Products
            .Where(p => p.BrandId == brandId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        var retrievedProduct = await GetProductByIdAsync(product.Id);
        try
        {
            UpdateProductProperties(retrievedProduct, product);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося оновити товар", e);
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }
    }

    private static void UpdateProductProperties(Product productFromDb, Product newProduct)
    {
        productFromDb.Name = newProduct.Name;
        productFromDb.Description = newProduct.Description;
        productFromDb.Price = newProduct.Price;
        productFromDb.ImageUrl = newProduct.ImageUrl;
        productFromDb.Stock = newProduct.Stock;
        productFromDb.CategoryId = newProduct.CategoryId;
        productFromDb.BrandId = newProduct.BrandId;
    }
}