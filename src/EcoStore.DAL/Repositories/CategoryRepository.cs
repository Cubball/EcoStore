using EcoStore.DAL.EF;
using EcoStore.DAL.Entities;
using EcoStore.DAL.Repositories.Exceptions;
using EcoStore.DAL.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace EcoStore.DAL.Repositories;

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

    public async Task<bool> CategoryExistsAsync(int id)
    {
        return (await _context.Categories.FindAsync(id)) is not null;
    }

    public async Task<bool> CategoryExistsAsync(string name)
    {
        return await _context.Categories.AnyAsync(c => c.Name == name);
    }

    private static void UpdateCategoryProperties(Category categoryFromDb, Category newCategory)
    {
        categoryFromDb.Name = newCategory.Name;
        categoryFromDb.Description = newCategory.Description;
    }
}