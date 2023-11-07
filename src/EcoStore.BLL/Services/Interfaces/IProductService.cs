using EcoStore.BLL.DTO;

namespace EcoStore.BLL.Services.Interfaces;

public interface IProductService
{
    Task<ProductDTO> GetProductByIdAsync(int id);

    Task<IEnumerable<ProductDTO>> GetProductsAsync(ProductsFilterDTO? filterDTO);

    Task<int> GetProductsCountAsync(ProductsFilterDTO? filterDTO);

    Task<int> CreateProductAsync(CreateProductDTO productDTO);

    Task UpdateProductAsync(UpdateProductDTO productDTO);

    Task DeleteProductAsync(int id);
}