using EcoStore.BLL.DTO;

namespace EcoStore.BLL.Services.Interfaces;

public interface IProductService
{
    Task<ProductDTO> GetProductByIdAsync(int id);

    Task<IEnumerable<ProductDTO>> GetProductsAsync();

    Task<int> CreateProductAsync(CreateUpdateProductDTO productDTO);

    Task UpdateProductAsync(CreateUpdateProductDTO productDTO);

    Task DeleteProductAsync(int id);
}