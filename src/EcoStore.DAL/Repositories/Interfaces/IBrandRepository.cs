using EcoStore.DAL.Entities;

namespace EcoStore.DAL.Repositories.Interfaces;

public interface IBrandRepository
{
    Task<IEnumerable<Brand>> GetBrandsAsync(int? skip = null, int? count = null);

    Task<Brand> GetBrandByIdAsync(int id);

    Task<int> AddBrandAsync(Brand brand);

    Task UpdateBrandAsync(int id, Action<Brand> updateAction);

    Task DeleteBrandAsync(int id);

    Task<bool> BrandExistsAsync(int id);

    Task<bool> BrandExistsAsync(string name);
}