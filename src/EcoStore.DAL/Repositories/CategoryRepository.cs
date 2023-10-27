using System.Linq.Expressions;

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

    public async Task<IEnumerable<Category>> GetCategoriesAsync(
            int? skip = null,
            int? count = null,
            Expression<Func<Category, object>>? orderBy = null,
            bool descending = false)
    {
        var categories = _context.Categories.AsQueryable();
        if (orderBy is not null)
        {
            categories = descending
                ? categories.OrderByDescending(orderBy)
                : categories.OrderBy(orderBy);
        }

        if (skip is not null)
        {
            categories = categories.Skip(skip.Value);
        }

        if (count is not null)
        {
            categories = categories.Take(count.Value);
        }

        return await categories.ToListAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new EntityNotFoundException($"Категорію з Id {id} не знайдено");
    }

    public async Task UpdateCategoryAsync(int id, Action<Category> updateAction)
    {
        var category = await GetCategoryByIdAsync(id);
        updateAction(category);
        try
        {
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
}