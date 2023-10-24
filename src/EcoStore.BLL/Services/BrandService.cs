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
    private readonly IValidator<BrandDTO> _brandValidator;

    public BrandService(IBrandRepository brandRepository, IValidator<BrandDTO> brandValidator)
    {
        _brandRepository = brandRepository;
        _brandValidator = brandValidator;
    }

    public async Task<int> CreateBrandAsync(BrandDTO brandDto)
    {
        await _brandValidator.ValidateAsync(brandDto);
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

    public async Task<IEnumerable<BrandDTO>> GetBrandsAsync()
    {
        return (await _brandRepository.GetBrandsAsync()).Select(b => b.ToDTO());
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsByBrandAsync(int brandId)
    {
        var brand = await _brandRepository.GetBrandByIdAsync(brandId);
        return brand.Products.Select(p => p.ToDTO());
    }

    public async Task UpdateBrandAsync(BrandDTO brandDto)
    {
        await _brandValidator.ValidateAsync(brandDto);
        try
        {
            await _brandRepository.UpdateBrandAsync(brandDto.ToEntity());
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }
}