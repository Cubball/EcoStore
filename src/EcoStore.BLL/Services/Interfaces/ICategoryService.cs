using EcoStore.BLL.DTO;

namespace EcoStore.BLL.Services.Interfaces;

public interface ICategoryService
{
    Task<CategoryDTO> GetCategoryAsync(int id);

    Task<IEnumerable<CategoryDTO>> GetCategorysAsync();

    Task<int> CreateCategoryAsync(CategoryDTO categoryDto);

    Task UpdateCategoryAsync(CategoryDTO categoryDto);

    Task DeleteCategoryAsync(int id);

    Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId);
}