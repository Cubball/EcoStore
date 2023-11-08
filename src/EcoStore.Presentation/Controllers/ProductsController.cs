using EcoStore.BLL.DTO;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.Presentation.Mapping;
using EcoStore.Presentation.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace EcoStore.Presentation.Controllers;

public class ProductsController : Controller
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 25;
    private const string DefaultSortBy = "Name";

    private readonly IProductService _productService;
    private readonly string _imagePath;

    public ProductsController(IProductService productService, IConfiguration configuration)
    {
        _productService = productService;
        _imagePath = configuration["Path:Images"]!;
    }

    public async Task<IActionResult> All(
        [FromQuery] int[] categories,
        [FromQuery] int[] brands,
        [FromQuery] int page = DefaultPageNumber,
        [FromQuery] int pageSize = DefaultPageSize,
        [FromQuery] int? minPrice = null,
        [FromQuery] int? maxPrice = null,
        [FromQuery] string? search = null,
        [FromQuery] string sortBy = DefaultSortBy,
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

    private static ProductsFilterDTO GetProductsFilter(
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
            page = DefaultPageNumber;
        }

        if (pageSize < 1)
        {
            pageSize = DefaultPageSize;
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