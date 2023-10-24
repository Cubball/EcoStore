using EcoStore.DAL.EF;
using EcoStore.DAL.Entities;
using EcoStore.DAL.Repositories.Exceptions;
using EcoStore.DAL.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace EcoStore.DAL.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly AppDbContext _context;

    public BrandRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddBrandAsync(Brand brand)
    {
        try
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return brand.Id;
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося додати бренд", e);
        }
    }

    public async Task DeleteBrandAsync(int id)
    {
        var brand = await GetBrandByIdAsync(id);
        try
        {
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося видалити бренд", e);
        }
    }

    public async Task<IEnumerable<Brand>> GetBrandsAsync()
    {
        return await _context.Brands.ToListAsync();
    }

    public async Task<Brand> GetBrandByIdAsync(int id)
    {
        return await _context.Brands
            .Include(b => b.Products)
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new EntityNotFoundException($"Бренд з Id {id} не знайдено");
    }

    public async Task UpdateBrandAsync(Brand brand)
    {
        var retrievedBrand = await GetBrandByIdAsync(brand.Id);
        try
        {
            UpdateBrandProperties(retrievedBrand, brand);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося оновити бренд", e);
        }
    }

    public async Task<bool> BrandExistsAsync(int id)
    {
        return (await _context.Brands.FindAsync(id)) is not null;
    }

    public async Task<bool> BrandExistsAsync(string name)
    {
        return await _context.Brands.AnyAsync(b => b.Name == name);
    }

    private static void UpdateBrandProperties(Brand brandFromDb, Brand newBrand)
    {
        brandFromDb.Name = newBrand.Name;
        brandFromDb.Description = newBrand.Description;
    }
}