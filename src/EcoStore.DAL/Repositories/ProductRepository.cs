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

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _context.Products.ToListAsync();
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