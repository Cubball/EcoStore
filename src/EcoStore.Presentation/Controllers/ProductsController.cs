using System.Globalization;

using EcoStore.BLL.DTO;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.Presentation.Mapping;
using EcoStore.Presentation.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace EcoStore.Presentation.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly int _defaultPageNumber;
    private readonly int _defaultPageSize;
    private readonly string _imagePath;

    public ProductsController(IProductService productService, IConfiguration configuration)
    {
        _productService = productService;
        _imagePath = configuration["Path:Images"]!;
        _defaultPageNumber = int.Parse(configuration["Defaults:PageNumber"]!, CultureInfo.InvariantCulture);
        _defaultPageSize = int.Parse(configuration["Defaults:PageSize"]!, CultureInfo.InvariantCulture);
    }

    public async Task<IActionResult> All(
        [FromQuery] int[] categories,
        [FromQuery] int[] brands,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] string sortBy,
        [FromQuery] int? minPrice = null,
        [FromQuery] int? maxPrice = null,
        [FromQuery] string? search = null,
        [FromQuery] bool descending = false)
    {
        var filter = GetProductsFilter(categories, brands, page, pageSize,
                minPrice, maxPrice, search, sortBy, descending);
        var products = (await _productService.GetProductsAsync(filter))
                .Select(p => p.ToViewModel());
        var productsCount = await _productService.GetProductsCountAsync(filter);
        foreach (var product in products)
        {
            product.ImagePath = Path.Combine(_imagePath, product.ImagePath);
        }

        var productsListViewModel = new ProductsListViewModel
        {
            PageInfo = new PageInfoViewModel
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = productsCount
            },
            Products = products
        };
        return View(productsListViewModel);
    }

    // TODO: catch?
    public async Task<IActionResult> Details(int id)
    {
        var product = (await _productService.GetProductByIdAsync(id)).ToViewModel();
        product.ImagePath = Path.Combine(_imagePath, product.ImagePath);
        return View(product);
    }

    private ProductsFilterDTO GetProductsFilter(
            int[] categories,
            int[] brands,
            int page,
            int pageSize,
            int? minPrice,
            int? maxPrice,
            string? search,
            string sortBy,
            bool descending)
    {
        if (!Enum.TryParse<SortBy>(sortBy, out var sortByEnum))
        {
            sortByEnum = SortBy.Name;
        }

        if (page < 1)
        {
            page = _defaultPageNumber;
        }

        if (pageSize < 1)
        {
            pageSize = _defaultPageSize;
        }

        return new ProductsFilterDTO
        {
            PageNumber = page,
            PageSize = pageSize,
            CategoryIds = categories,
            BrandIds = brands,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            SearchString = search,
            SortBy = sortByEnum,
            Descending = descending
        };
    }
}