using EcoStore.DAL.Entities;

namespace EcoStore.DAL.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategoriesAsync();

    Task<Category> GetCategoryByIdAsync(int id);

    Task<int> AddCategoryAsync(Category category);

    Task UpdateCategoryAsync(Category category);

    Task DeleteCategoryAsync(int id);

    Task<bool> CategoryExistsAsync(int id);

    Task<bool> CategoryExistsAsync(string name);
}