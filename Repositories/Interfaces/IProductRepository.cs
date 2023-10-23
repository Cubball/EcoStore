using Project.Models;

namespace Project.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();

    Task<Product> GetProductByIdAsync(int id);

    Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);

    Task<IEnumerable<Product>> GetProductsByBrandIdAsync(int brandId);

    Task<int> AddProductAsync(Product product);

    Task UpdateProductAsync(Product product);

    Task DeleteProductAsync(int id);
}