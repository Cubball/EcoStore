using Project.Models;

namespace Project.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategoriesAsync();

    Task<Category> GetCategoryByIdAsync(int id);

    Task<int> AddCategoryAsync(Category category);

    Task UpdateCategoryAsync(Category category);

    Task DeleteCategoryAsync(int id);
}