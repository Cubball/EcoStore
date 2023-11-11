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
    private readonly IReportService _reportService;
    private readonly int _defaultPageNumber;
    private readonly int _defaultPageSize;
    private readonly string _imagePath;

    public ProductsController(
            IProductService productService,
            IReportService reportService,
            IConfiguration configuration)
    {
        _productService = productService;
        _imagePath = configuration["Path:Images"]!;
        _defaultPageNumber = int.Parse(configuration["Defaults:PageNumber"]!, CultureInfo.InvariantCulture);
        _defaultPageSize = int.Parse(configuration["Defaults:PageSize"]!, CultureInfo.InvariantCulture);
        _reportService = reportService;
    }

    public async Task<IActionResult> All(
        [FromQuery] int[] categories,
        [FromQuery] int[] brands,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] SortProductsByViewModel sortBy,
        [FromQuery] int? minPrice = null,
        [FromQuery] int? maxPrice = null,
        [FromQuery] string? search = null,
        [FromQuery] bool descending = false)
    {
        var filter = GetProductsFilter(categories, brands, page, pageSize,
                minPrice, maxPrice, search, sortBy, descending);
        var products = (await _productService.GetProductsAsync(filter))
                .Select(p => p.ToViewModel())
                .ToList();
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

    public async Task<IActionResult> Report()
    {
        var (content, fileName) = await _reportService.GetSalesReportAsync(SortSalesByDTO.Name, true, DateTime.Now.AddDays(-7));
        return File(content, "text/html", fileName);
    }

    private ProductsFilterDTO GetProductsFilter(
            int[] categories,
            int[] brands,
            int page,
            int pageSize,
            int? minPrice,
            int? maxPrice,
            string? search,
            SortProductsByViewModel sortBy,
            bool descending)
    {

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
            SortBy = sortBy.ToDTO(),
            Descending = descending
        };
    }
}