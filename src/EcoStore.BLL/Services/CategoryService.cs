using EcoStore.BLL.DTO;
using EcoStore.BLL.Mapping;
using EcoStore.BLL.Services.Exceptions;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.BLL.Validation.Interfaces;
using EcoStore.DAL.Repositories.Exceptions;
using EcoStore.DAL.Repositories.Interfaces;

namespace EcoStore.BLL.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<CreateCategoryDTO> _createCategoryValidator;
    private readonly IValidator<UpdateCategoryDTO> _updateCategoryValidator;

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

    public async Task<IEnumerable<CategoryDTO>> GetCategorysAsync()
    {
        return (await _categoryRepository.GetCategoriesAsync()).Select(b => b.ToDTO());
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
            await _categoryRepository.UpdateCategoryAsync(categoryDto.ToEntity());
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }
}