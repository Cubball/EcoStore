using EcoStore.BLL.DTO;

namespace EcoStore.BLL.Services.Interfaces;

public interface IBrandService
{
    Task<BrandDTO> GetBrandAsync(int id);

    Task<IEnumerable<BrandDTO>> GetAllBrandsAsync();

    Task<IEnumerable<BrandDTO>> GetBrandsAsync(int pageNumber, int pageSize);

    Task<int> GetBrandsCountAsync();

    Task<int> CreateBrandAsync(CreateBrandDTO brandDto);

    Task UpdateBrandAsync(UpdateBrandDTO brandDto);

    Task DeleteBrandAsync(int id);
}