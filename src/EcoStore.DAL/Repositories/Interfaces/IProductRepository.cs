using System.Linq.Expressions;

using EcoStore.DAL.Entities;

namespace EcoStore.DAL.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync(
            int? skip = null,
            int? count = null,
            Expression<Func<Product, bool>>? predicate = null,
            Expression<Func<Product, object>>? orderBy = null,
            bool descending = false);

    Task<Product> GetProductByIdAsync(int id);

    Task<int> AddProductAsync(Product product);

    Task UpdateProductAsync(int id, Action<Product> updateAction);

    Task DeleteProductAsync(Product product);

    Task<bool> ProductExistsAsync(int id);

    Task<bool> ProductExistsAsync(string name);
}