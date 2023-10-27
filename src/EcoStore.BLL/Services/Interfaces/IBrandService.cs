using EcoStore.BLL.DTO;

namespace EcoStore.BLL.Services.Interfaces;

public interface IBrandService
{
    Task<BrandDTO> GetBrandAsync(int id);

    Task<IEnumerable<BrandDTO>> GetAllBrandsAsync();

    Task<IEnumerable<BrandDTO>> GetBrandsAsync(int? pageNumber = null, int? pageSize = null);

    Task<int> CreateBrandAsync(CreateBrandDTO brandDto);

    Task UpdateBrandAsync(UpdateBrandDTO brandDto);

    Task DeleteBrandAsync(int id);

    Task<IEnumerable<ProductDTO>> GetProductsByBrandAsync(int brandId);
}