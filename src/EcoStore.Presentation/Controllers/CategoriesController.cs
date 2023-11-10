using System.Globalization;

using EcoStore.BLL.DTO;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.Presentation.Mapping;
using EcoStore.Presentation.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace EcoStore.Presentation.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;
    private readonly int _defaultPageNumber;
    private readonly int _defaultPageSize;
    private readonly string _imagePath;

    public CategoriesController(ICategoryService categoryService,
            IProductService productService,
            IConfiguration configuration)
    {
        _categoryService = categoryService;
        _productService = productService;
        _imagePath = configuration["Path:Images"]!;
        _defaultPageNumber = int.Parse(configuration["Defaults:PageNumber"]!, CultureInfo.InvariantCulture);
        _defaultPageSize = int.Parse(configuration["Defaults:PageSize"]!, CultureInfo.InvariantCulture);
    }

    // TODO: catch?
    public async Task<IActionResult> Details(int id)
    {
        var category = (await _categoryService.GetCategoryAsync(id)).ToViewModel();
        var filter = new ProductsFilterDTO
        {
            PageNumber = _defaultPageNumber,
            PageSize = _defaultPageSize,
            CategoryIds = new int[] { category.Id },
        };
        var categoryProducts = (await _productService.GetProductsAsync(filter))
            .Select(p => p.ToViewModel())
            .ToList();
        foreach (var product in categoryProducts)
        {
            product.ImagePath = Path.Combine(_imagePath, product.ImagePath);
        }

        return View(new CategoryDetailsViewModel
        {
            Category = category,
            Products = categoryProducts,
        });
    }
}