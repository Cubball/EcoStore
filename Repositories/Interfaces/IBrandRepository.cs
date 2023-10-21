using Project.Models;

namespace Project.Repositories.Interfaces;

public interface IBrandRepository : IDisposable
{
    Task<IEnumerable<Brand>> GetBrandsAsync();

    Task<Brand> GetBrandByIdAsync(int id);

    Task<int> AddBrandAsync(Brand brand);

    Task UpdateBrandAsync(Brand brand);

    Task DeleteBrandAsync(int id);
}