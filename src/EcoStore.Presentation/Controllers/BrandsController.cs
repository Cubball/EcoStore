using System.Globalization;

using EcoStore.BLL.DTO;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.Presentation.Mapping;
using EcoStore.Presentation.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace EcoStore.Presentation.Controllers;

public class BrandsController : Controller
{
    private readonly IBrandService _brandService;
    private readonly IProductService _productService;
    private readonly int _defaultPageNumber;
    private readonly int _defaultPageSize;
    private readonly string _imagePath;

    public BrandsController(IBrandService brandService,
            IProductService productService,
            IConfiguration configuration)
    {
        _brandService = brandService;
        _productService = productService;
        _imagePath = configuration["Path:Images"]!;
        _defaultPageNumber = int.Parse(configuration["Defaults:PageNumber"]!, CultureInfo.InvariantCulture);
        _defaultPageSize = int.Parse(configuration["Defaults:PageSize"]!, CultureInfo.InvariantCulture);
    }

    // TODO: catch?
    public async Task<IActionResult> Details(int id)
    {
        var brand = (await _brandService.GetBrandAsync(id)).ToViewModel();
        var filter = new ProductsFilterDTO
        {
            PageNumber = _defaultPageNumber,
            PageSize = _defaultPageSize,
            BrandIds = new int[] { id },
            OnlyAvailable = true,
        };
        var brandProducts = (await _productService.GetProductsAsync(filter))
            .Select(p => p.ToViewModel())
            .ToList();
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