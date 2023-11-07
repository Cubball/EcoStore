using System.Linq.Expressions;

using EcoStore.DAL.Entities;

namespace EcoStore.DAL.Repositories.Interfaces;

public interface IBrandRepository
{
    Task<IEnumerable<Brand>> GetBrandsAsync(
            int? skip = null,
            int? count = null,
            Expression<Func<Brand, object>>? orderBy = null,
            bool descending = false);

    Task<int> GetBrandsCountAsync();

    Task<Brand> GetBrandByIdAsync(int id);

    Task<int> AddBrandAsync(Brand brand);

    Task UpdateBrandAsync(int id, Action<Brand> updateAction);

    Task DeleteBrandAsync(int id);

    Task<bool> BrandExistsAsync(int id);

    Task<bool> BrandExistsAsync(string name);
}