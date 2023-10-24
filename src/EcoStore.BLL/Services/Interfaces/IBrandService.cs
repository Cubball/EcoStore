using EcoStore.BLL.DTO;

namespace EcoStore.BLL.Services.Interfaces;

public interface IBrandService
{
    Task<BrandDTO> GetBrandAsync(int id);

    Task<IEnumerable<BrandDTO>> GetBrandsAsync();

    Task<int> CreateBrandAsync(BrandDTO brandDto);

    Task UpdateBrandAsync(BrandDTO brandDto);

    Task DeleteBrandAsync(int id);

    // TODO: Add GetProductsByBrandAsync
}