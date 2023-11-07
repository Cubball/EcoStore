using EcoStore.BLL.DTO;

namespace EcoStore.BLL.Services.Interfaces;

public interface IProductService
{
    Task<ProductDTO> GetProductByIdAsync(int id);

    Task<IEnumerable<ProductDTO>> GetProductsAsync(ProductsFilterDTO? filterDTO = null);

    Task<int> GetProductsCountAsync(ProductsFilterDTO? filterDTO = null);

    Task<int> CreateProductAsync(CreateProductDTO productDTO);

    Task UpdateProductAsync(UpdateProductDTO productDTO);

    Task DeleteProductAsync(int id);
}