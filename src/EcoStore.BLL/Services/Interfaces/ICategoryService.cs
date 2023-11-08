using EcoStore.BLL.DTO;

namespace EcoStore.BLL.Services.Interfaces;

public interface ICategoryService
{
    Task<CategoryDTO> GetCategoryAsync(int id);

    Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();

    Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(int pageNumber, int pageSize);

    Task<int> GetCategoriesCountAsync();

    Task<int> CreateCategoryAsync(CreateCategoryDTO categoryDto);

    Task UpdateCategoryAsync(UpdateCategoryDTO categoryDto);

    Task DeleteCategoryAsync(int id);
}