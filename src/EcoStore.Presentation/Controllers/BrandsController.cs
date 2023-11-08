using EcoStore.BLL.DTO;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.Presentation.Mapping;
using EcoStore.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcoStore.Presentation.Controllers;

public class BrandsController : Controller
{
    // TODO: move to appsettings.json or a static class?
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 25;

    private readonly IBrandService _brandService;
    private readonly IProductService _productService;
    private readonly string _imagePath;

    public BrandsController(IBrandService brandService,
            IProductService productService,
            IConfiguration configuration)
    {
        _brandService = brandService;
        _productService = productService;
        _imagePath = configuration["Path:Images"]!;
    }

    // TODO: do i need this endpoint?
    public async Task<IActionResult> All(
        [FromQuery] int page = DefaultPageNumber,
        [FromQuery] int pageSize = DefaultPageSize)
    {
        if (page < 1)
        {
            page = DefaultPageNumber;
        }

        if (pageSize < 1)
        {
            pageSize = DefaultPageSize;
        }

        var brands = (await _brandService.GetBrandsAsync(page, pageSize)).Select(b => b.ToViewModel());
        return View(brands);
    }

    // TODO: catch?
    public async Task<IActionResult> Details(int id)
    {
        var brand = (await _brandService.GetBrandAsync(id)).ToViewModel();
        var filter = new ProductsFilterDTO
        {
            PageNumber = DefaultPageNumber,
            PageSize = DefaultPageSize,
            BrandIds = new int[] { id },
        };
        var brandProducts = (await _productService.GetProductsAsync(filter)).Select(p => p.ToViewModel());
        foreach (var product in brandProducts)
        {
            product.ImagePath = Path.Combine(_imagePath, product.ImagePath);
        }

        return View(new BrandDetailsViewModel
        {
            Brand = brand,
            Products = brandProducts,
        });
    }
}