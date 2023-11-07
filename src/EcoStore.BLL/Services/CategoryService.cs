using System.Linq.Expressions;

using EcoStore.BLL.DTO;
using EcoStore.BLL.Mapping;
using EcoStore.BLL.Services.Exceptions;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.BLL.Validation.Interfaces;
using EcoStore.DAL.Entities;
using EcoStore.DAL.Repositories.Exceptions;
using EcoStore.DAL.Repositories.Interfaces;

namespace EcoStore.BLL.Services;

public class CategoryService : ICategoryService
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 25;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<CreateCategoryDTO> _createCategoryValidator;
    private readonly IValidator<UpdateCategoryDTO> _updateCategoryValidator;

    private Expression<Func<Category, object>>? _orderBy;

    public CategoryService(ICategoryRepository categoryRepository,
            IValidator<CreateCategoryDTO> createCategoryValidator,
            IValidator<UpdateCategoryDTO> updateCategoryValidator)
    {
        _categoryRepository = categoryRepository;
        _createCategoryValidator = createCategoryValidator;
        _updateCategoryValidator = updateCategoryValidator;
    }

    public async Task<int> CreateCategoryAsync(CreateCategoryDTO categoryDto)
    {
        await _createCategoryValidator.ValidateAsync(categoryDto);
        try
        {
            return await _categoryRepository.AddCategoryAsync(categoryDto.ToEntity());
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }

    public async Task DeleteCategoryAsync(int id)
    {
        try
        {
            await _categoryRepository.DeleteCategoryAsync(id);
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
        catch (EntityNotFoundException e)
        {
            throw new ObjectNotFoundException(e.Message, e);
        }
    }

    public async Task<CategoryDTO> GetCategoryAsync(int id)
    {
        try
        {
            return (await _categoryRepository.GetCategoryByIdAsync(id)).ToDTO();
        }
        catch (EntityNotFoundException e)
        {
            throw new ObjectNotFoundException(e.Message, e);
        }
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
    {
        _orderBy ??= c => c.Name;
        return (await _categoryRepository.GetCategoriesAsync(orderBy: _orderBy)).Select(b => b.ToDTO());
    }

    public async Task<int> GetCategoriesCountAsync()
    {
        return await _categoryRepository.GetCategoriesCountAsync();
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
        return category.Products.Select(p => p.ToDTO());
    }

    public async Task UpdateCategoryAsync(UpdateCategoryDTO categoryDto)
    {
        await _updateCategoryValidator.ValidateAsync(categoryDto);
        try
        {
            await _categoryRepository.UpdateCategoryAsync(categoryDto.Id, c =>
            {
                c.Name = categoryDto.Name;
                c.Description = categoryDto.Description;
            });
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }

    public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(int? pageNumber = null, int? pageSize = null)
    {
        if (pageNumber is null or < 1)
        {
            pageNumber = DefaultPageNumber;
        }

        if (pageSize is null or < 1)
        {
            pageSize = DefaultPageSize;
        }

        _orderBy ??= c => c.Name;
        var skip = (pageNumber - 1) * pageSize;
        var categories = await _categoryRepository.GetCategoriesAsync(skip: skip, count: pageSize, orderBy: _orderBy);
        return categories.Select(c => c.ToDTO());
    }
}