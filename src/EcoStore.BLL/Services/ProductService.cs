using EcoStore.BLL.DTO;
using EcoStore.BLL.Mapping;
using EcoStore.BLL.Services.Exceptions;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.BLL.Validation.Interfaces;
using EcoStore.DAL.Repositories.Exceptions;
using EcoStore.DAL.Repositories.Interfaces;

namespace EcoStore.BLL.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<CreateUpdateProductDTO> _productValidator;

    public ProductService(IProductRepository productRepository, IValidator<CreateUpdateProductDTO> productValidator)
    {
        _productRepository = productRepository;
        _productValidator = productValidator;
    }

    public async Task<int> CreateProductAsync(CreateUpdateProductDTO productDTO)
    {
        await _productValidator.ValidateAsync(productDTO);
        try
        {
            return await _productRepository.AddProductAsync(productDTO.ToEntity());
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        try
        {
            await _productRepository.DeleteProductAsync(id);
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

    public async Task<ProductDTO> GetProductByIdAsync(int id)
    {
        try
        {
            return (await _productRepository.GetProductByIdAsync(id)).ToDTO();
        }
        catch (EntityNotFoundException e)
        {
            throw new ObjectNotFoundException(e.Message, e);
        }
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsAsync()
    {
        return (await _productRepository.GetProductsAsync()).Select(p => p.ToDTO());
    }

    public async Task UpdateProductAsync(CreateUpdateProductDTO productDTO)
    {
        await _productValidator.ValidateAsync(productDTO);
        try
        {
            await _productRepository.UpdateProductAsync(productDTO.ToEntity());
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }
}
