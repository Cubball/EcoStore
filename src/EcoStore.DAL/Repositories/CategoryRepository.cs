using Microsoft.EntityFrameworkCore;

using Project.Data;
using Project.Models;
using Project.Repositories.Exceptions;
using Project.Repositories.Interfaces;

namespace Project.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddCategoryAsync(Category category)
    {
        try
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося додати категорію", e);
        }
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await GetCategoryByIdAsync(id);
        try
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося видалити категорію", e);
        }
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new EntityNotFoundException($"Категорію з Id {id} не знайдено");
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        var retrievedCategory = await GetCategoryByIdAsync(category.Id);
        try
        {
            UpdateCategoryProperties(retrievedCategory, category);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося оновити категорію", e);
        }
    }

    private static void UpdateCategoryProperties(Category categoryFromDb, Category newCategory)
    {
        categoryFromDb.Name = newCategory.Name;
        categoryFromDb.Description = newCategory.Description;
    }
}