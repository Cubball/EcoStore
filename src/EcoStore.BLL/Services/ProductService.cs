using System.Linq.Expressions;

using EcoStore.BLL.DTO;
using EcoStore.BLL.Infrastructure;
using EcoStore.BLL.Mapping;
using EcoStore.BLL.Services.Exceptions;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.BLL.Validation.Interfaces;
using EcoStore.DAL.Entities;
using EcoStore.DAL.Files.Exceptions;
using EcoStore.DAL.Files.Interfaces;
using EcoStore.DAL.Repositories.Exceptions;
using EcoStore.DAL.Repositories.Interfaces;

namespace EcoStore.BLL.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IFileManager _fileManager;
    private readonly IGuidProvider _guidProvider;
    private readonly IValidator<CreateProductDTO> _createProductValidator;
    private readonly IValidator<UpdateProductDTO> _updateProductValidator;

    public ProductService(IProductRepository productRepository,
            IFileManager fileManager,
            IGuidProvider guidProvider,
            IValidator<CreateProductDTO> createProductValidator,
            IValidator<UpdateProductDTO> updateProductValidator)
    {
        _productRepository = productRepository;
        _fileManager = fileManager;
        _guidProvider = guidProvider;
        _createProductValidator = createProductValidator;
        _updateProductValidator = updateProductValidator;
    }

    public async Task<int> CreateProductAsync(CreateProductDTO productDTO)
    {
        await _createProductValidator.ValidateAsync(productDTO);
        try
        {
            var fileName = $"img_{_guidProvider.NewGuid()}{productDTO.ImageExtension}";
            await _fileManager.SaveFileAsync(productDTO.ImageStream, fileName);
            var product = productDTO.ToEntity();
            product.ImageName = fileName;
            return await _productRepository.AddProductAsync(product);
        }
        catch (FileUploadFailedException e)
        {
            throw new ServiceException(e.Message, e);
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
            var product = await _productRepository.GetProductByIdAsync(id);
            _fileManager.DeleteFile(product.ImageName);
            await _productRepository.DeleteProductAsync(product);
        }
        catch (EntityNotFoundException e)
        {
            throw new ObjectNotFoundException(e.Message, e);
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
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

    public async Task<IEnumerable<ProductDTO>> GetProductsAsync(ProductsFilterDTO filterDTO)
    {
        var skip = (filterDTO.PageNumber - 1) * filterDTO.PageSize;
        var predicates = GetFilterPredicates(filterDTO);
        var orderBySelector = GetOrderBySelector(filterDTO);
        var descending = filterDTO.Descending;
        var products = await _productRepository.GetProductsAsync(
                skip: skip,
                count: filterDTO.PageSize,
                predicates: predicates,
                orderBy: orderBySelector,
                descending: descending);
        return products.Select(p => p.ToDTO());
    }

    public async Task<int> GetProductsCountAsync(ProductsFilterDTO filterDTO)
    {
        return await _productRepository.GetProductsCountAsync(GetFilterPredicates(filterDTO));
    }

    public async Task UpdateProductAsync(UpdateProductDTO productDTO)
    {
        await _updateProductValidator.ValidateAsync(productDTO);
        try
        {
            var newFileName = productDTO.ImageStream is not null
                ? await UpdateProductImage(productDTO)
                : null;
            await _productRepository.UpdateProductAsync(productDTO.Id, p =>
            {
                p.Name = productDTO.Name;
                p.Description = productDTO.Description;
                p.Price = productDTO.Price;
                p.Stock = productDTO.Stock;
                p.BrandId = productDTO.BrandId;
                p.CategoryId = productDTO.CategoryId;
                p.ImageName = newFileName ?? p.ImageName;
            });
        }
        catch (RepositoryException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }

    private async Task<string> UpdateProductImage(UpdateProductDTO productDTO)
    {
        var oldProduct = await _productRepository.GetProductByIdAsync(productDTO.Id);
        _fileManager.DeleteFile(oldProduct.ImageName);
        var fileName = $"img_{_guidProvider.NewGuid()}{productDTO.ImageExtension}";
        try
        {
            await _fileManager.SaveFileAsync(productDTO.ImageStream!, fileName);
            return fileName;
        }
        catch (FileUploadFailedException e)
        {
            throw new ServiceException(e.Message, e);
        }
    }

    private static Expression<Func<Product, object>>? GetOrderBySelector(ProductsFilterDTO filterDTO)
    {
        return filterDTO.SortBy switch
        {
            SortByDTO.Price => p => p.Price,
            SortByDTO.Name => p => p.Name,
            SortByDTO.DateCreated => p => p.Id,
            _ => null
        };
    }

    private static IEnumerable<Expression<Func<Product, bool>>>? GetFilterPredicates(ProductsFilterDTO filterDTO)
    {
        var predicates = new List<Expression<Func<Product, bool>>>();
        if (filterDTO.CategoryIds is not null && filterDTO.CategoryIds.Length > 0)
        {
            predicates.Add(p => filterDTO.CategoryIds.Contains(p.CategoryId));
        }

        if (filterDTO.BrandIds is not null && filterDTO.BrandIds.Length > 0)
        {
            predicates.Add(p => filterDTO.BrandIds.Contains(p.BrandId));
        }

        if (filterDTO.MinPrice is not null)
        {
            predicates.Add(p => p.Price >= filterDTO.MinPrice);
        }

        if (filterDTO.MaxPrice is not null)
        {
            predicates.Add(p => p.Price <= filterDTO.MaxPrice);
        }

        if (!string.IsNullOrWhiteSpace(filterDTO.SearchString))
        {
            predicates.Add(p => p.Name.Contains(filterDTO.SearchString));
        }

        if (filterDTO.OnlyAvailable)
        {
            predicates.Add(p => p.Stock > 0);
        }

        return predicates;
    }
}