using EcoStore.BLL.DTO;
using EcoStore.BLL.Mapping;
using EcoStore.BLL.Services.Exceptions;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.BLL.Validation.Interfaces;
using EcoStore.DAL.Repositories.Exceptions;
using EcoStore.DAL.Repositories.Interfaces;

namespace EcoStore.BLL.Services;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _brandRepository;
    private readonly IValidator<CreateBrandDTO> _createBrandValidator;
    private readonly IValidator<UpdateBrandDTO> _updateBrandValidator;

    public BrandService(IBrandRepository brandRepository,
            IValidator<CreateBrandDTO> createBrandValidator,
            IValidator<UpdateBrandDTO> updateBrandValidator)
    {
        _brandRepository = brandRepository;
        _createBrandValidator = createBrandValidator;
        _updateBrandValidator = updateBrandValidator;
    }

    public async Task<int> CreateBrandAsync(CreateBrandDTO brandDto)
    {
        await _createBrandValidator.ValidateAsync(brandDto);
        try
        {
            return await _brandRepository.AddBrandAsync(brandDto.ToEntity());
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }

    public async Task DeleteBrandAsync(int id)
    {
        try
        {
            await _brandRepository.DeleteBrandAsync(id);
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

    public async Task<BrandDTO> GetBrandAsync(int id)
    {
        try
        {
            return (await _brandRepository.GetBrandByIdAsync(id)).ToDTO();
        }
        catch (EntityNotFoundException e)
        {
            throw new ObjectNotFoundException(e.Message, e);
        }
    }

    public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
    {
        return (await _brandRepository.GetBrandsAsync()).Select(b => b.ToDTO());
    }

    // TODO: magic numbers
    public async Task<IEnumerable<BrandDTO>> GetBrandsAsync(int? pageNumber = null, int? pageSize = null)
    {
        pageNumber ??= 1;
        pageSize ??= 25;
        var skip = (pageNumber - 1) * pageSize;
        var brands = await _brandRepository.GetBrandsAsync(skip: skip, count: pageSize);
        return brands.Select(b => b.ToDTO());
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsByBrandAsync(int brandId)
    {
        var brand = await _brandRepository.GetBrandByIdAsync(brandId);
        return brand.Products.Select(p => p.ToDTO());
    }

    public async Task UpdateBrandAsync(UpdateBrandDTO brandDto)
    {
        await _updateBrandValidator.ValidateAsync(brandDto);
        try
        {
            await _brandRepository.UpdateBrandAsync(brandDto.Id, b =>
            {
                b.Name = brandDto.Name;
                b.Description = brandDto.Description;
            });
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }
}