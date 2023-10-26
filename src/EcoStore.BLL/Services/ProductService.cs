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
    private readonly IValidator<CreateProductDTO> _createProductValidator;
    private readonly IValidator<UpdateProductDTO> _updateProductValidator;

    public ProductService(IProductRepository productRepository,
            IValidator<CreateProductDTO> createProductValidator,
            IValidator<UpdateProductDTO> updateProductValidator)
    {
        _productRepository = productRepository;
        _createProductValidator = createProductValidator;
        _updateProductValidator = updateProductValidator;
    }

    public async Task<int> CreateProductAsync(CreateProductDTO productDTO)
    {
        await _createProductValidator.ValidateAsync(productDTO);
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

    public async Task UpdateProductAsync(UpdateProductDTO productDTO)
    {
        await _updateProductValidator.ValidateAsync(productDTO);
        try
        {
            await _productRepository.UpdateProductAsync(productDTO.Id, p =>
            {
                p.Name = productDTO.Name;
                p.Description = productDTO.Description;
                p.Price = productDTO.Price;
                p.ImageUrl = productDTO.ImageUrl;
                p.Stock = productDTO.Stock;
                p.BrandId = productDTO.BrandId;
                p.CategoryId = productDTO.CategoryId;
            });
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }
}