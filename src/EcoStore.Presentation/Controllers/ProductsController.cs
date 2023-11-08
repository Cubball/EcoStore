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

    public async Task<IActionResult> Index(
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
        if (sortBy is null || !Enum.TryParse<SortBy>(sortBy, out var sortByEnum))
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

        var filter = new ProductsFilterDTO
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
        var products = await _productService.GetProductsAsync(filter);
        var productViewModels = new List<ProductViewModel>();
        var productsCount = await _productService.GetProductsCountAsync(filter);
        foreach (var product in products)
        {
            var viewModel = product.ToViewModel();
            viewModel.ImagePath = Path.Combine(_imagePath, product.ImageName);
            productViewModels.Add(viewModel);
        }

        var productsListViewModel = new ProductsListViewModel
        {
            PageInfo = new PageInfoViewModel
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = productsCount
            },
            Products = productViewModels
        };
        return View(productsListViewModel);
    }
}