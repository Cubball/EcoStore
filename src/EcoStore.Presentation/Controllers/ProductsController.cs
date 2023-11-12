using System.Globalization;

using EcoStore.BLL.DTO;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.Presentation.Mapping;
using EcoStore.Presentation.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcoStore.Presentation.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly IBrandService _brandService;
    private readonly ICategoryService _categoryService;
    private readonly int _defaultPageNumber;
    private readonly int _defaultPageSize;
    private readonly string _imagePath;

    public ProductsController(
            IProductService productService,
            IBrandService brandService,
            ICategoryService categoryService,
            IConfiguration configuration)
    {
        _productService = productService;
        _brandService = brandService;
        _categoryService = categoryService;
        _imagePath = configuration["Path:Images"]!;
        _defaultPageNumber = int.Parse(configuration["Defaults:PageNumber"]!, CultureInfo.InvariantCulture);
        _defaultPageSize = int.Parse(configuration["Defaults:PageSize"]!, CultureInfo.InvariantCulture);
    }

    public async Task<IActionResult> All(ProductsListViewModel productsViewModel,
            [FromQuery] int page, [FromQuery] int pageSize)
    {
        var filter = GetProductsFilter(productsViewModel.Filter, page, pageSize);
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
            Products = products,
            Brands = (await _brandService.GetAllBrandsAsync())
                    .Select(b => new SelectListItem(b.Name, b.Id.ToString(CultureInfo.InvariantCulture))),
            Categories = (await _categoryService.GetAllCategoriesAsync())
                    .Select(c => new SelectListItem(c.Name, c.Id.ToString(CultureInfo.InvariantCulture))),
            Filter = productsViewModel.Filter,
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

    private ProductsFilterDTO GetProductsFilter(ProductFilterViewModel filter, int page, int pageSize)
    {
        if (page < 1)
        {
            page = _defaultPageNumber;
        }

        if (pageSize < 1)
        {
            pageSize = _defaultPageSize;
        }

        var (sortByDTO, descending) = filter?.SortBy.ToDTO() ?? (SortByDTO.Name, false);
        return new ProductsFilterDTO
        {
            PageNumber = page,
            PageSize = pageSize,
            CategoryIds = filter?.Categories,
            BrandIds = filter?.Brands,
            MinPrice = filter?.MinPrice,
            MaxPrice = filter?.MaxPrice,
            SearchString = filter?.Search,
            SortBy = sortByDTO,
            Descending = descending,
            OnlyAvailable = true,
        };
    }
}