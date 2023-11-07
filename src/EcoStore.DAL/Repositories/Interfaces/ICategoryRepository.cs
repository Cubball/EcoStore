using System.Linq.Expressions;

using EcoStore.DAL.Entities;

namespace EcoStore.DAL.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategoriesAsync(
            int? skip = null,
            int? count = null,
            Expression<Func<Category, object>>? orderBy = null,
            bool descending = false);

    Task<Category> GetCategoryByIdAsync(int id);

    Task<int> GetCategoriesCountAsync();

    Task<int> AddCategoryAsync(Category category);

    Task UpdateCategoryAsync(int id, Action<Category> updateAction);

    Task DeleteCategoryAsync(int id);

    Task<bool> CategoryExistsAsync(int id);

    Task<bool> CategoryExistsAsync(string name);
}