using EcoStore.DAL.Entities;

namespace EcoStore.DAL.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();

    Task<Product> GetProductByIdAsync(int id);

    Task<int> AddProductAsync(Product product);

    Task UpdateProductAsync(int id, Action<Product> updateAction);

    Task DeleteProductAsync(int id);

    Task<bool> ProductExistsAsync(int id);

    Task<bool> ProductExistsAsync(string name);
}