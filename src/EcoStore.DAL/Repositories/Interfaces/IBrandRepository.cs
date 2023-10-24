using EcoStore.DAL.Entities;

namespace EcoStore.DAL.Repositories.Interfaces;

public interface IBrandRepository
{
    Task<IEnumerable<Brand>> GetBrandsAsync();

    Task<Brand> GetBrandByIdAsync(int id);

    Task<int> AddBrandAsync(Brand brand);

    Task UpdateBrandAsync(Brand brand);

    Task DeleteBrandAsync(int id);

    Task<bool> BrandExistsAsync(int id);

    Task<bool> BrandExistsAsync(string name);
}