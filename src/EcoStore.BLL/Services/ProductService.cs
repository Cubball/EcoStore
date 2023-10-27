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

public class ProductService : IProductService
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 25;
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

    public async Task<IEnumerable<ProductDTO>> GetProductsAsync(ProductsFilterDTO? filterDTO = null)
    {
        int pageNumber = DefaultPageNumber;
        int pageSize = DefaultPageSize;
        if (filterDTO?.PageNumber is not null and > 0)
        {
            pageNumber = filterDTO.PageNumber.Value;
        }

        if (filterDTO?.PageSize is not null and > 0)
        {
            pageSize = filterDTO.PageSize.Value;
        }

        var skip = (pageNumber - 1) * pageSize;
        var predicate = GetFilterPredicate(filterDTO);
        var orderBySelector = GetOrderBySelector(filterDTO);
        var ascending = filterDTO?.Descending ?? false;
        var products = await _productRepository.GetProductsAsync(
                skip: skip,
                count: pageSize,
                predicate: predicate,
                orderBy: orderBySelector,
                ascending: ascending);
        return products.Select(p => p.ToDTO());
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

    private static Expression<Func<Product, bool>> Combine(Expression<Func<Product, bool>>? first, Expression<Func<Product, bool>> second)
    {
        if (first is null)
        {
            return second;
        }
        var body = Expression.AndAlso(first.Body, second.Body);
        return Expression.Lambda<Func<Product, bool>>(body, first.Parameters);
    }

    private static Expression<Func<Product, object>>? GetOrderBySelector(ProductsFilterDTO? filterDTO)
    {
        return filterDTO?.SortBy is null
            ? null
            : filterDTO.SortBy switch
            {
                SortBy.Price => p => p.Price,
                SortBy.Name => p => p.Name,
                SortBy.DateCreated => p => p.Id,
                _ => null
            };
    }

    private static Expression<Func<Product, bool>>? GetFilterPredicate(ProductsFilterDTO? filterDTO)
    {
        Expression<Func<Product, bool>>? predicate = null;
        if (filterDTO?.CategoryIds is not null)
        {
            predicate = Combine(predicate, p => filterDTO.CategoryIds.Contains(p.CategoryId));
        }

        if (filterDTO?.BrandIds is not null)
        {
            predicate = Combine(predicate, p => filterDTO.BrandIds.Contains(p.BrandId));
        }

        if (filterDTO?.MinPrice is not null)
        {
            predicate = Combine(predicate, p => p.Price >= filterDTO.MinPrice);
        }

        if (filterDTO?.MaxPrice is not null)
        {
            predicate = Combine(predicate, p => p.Price <= filterDTO.MaxPrice);
        }

        if (filterDTO?.SearchString is not null)
        {
            predicate = Combine(predicate, p => p.Name.Contains(filterDTO.SearchString, StringComparison.InvariantCultureIgnoreCase));
        }

        return predicate;
    }
}