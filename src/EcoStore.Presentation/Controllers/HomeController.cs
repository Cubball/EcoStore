using System.Globalization;

using EcoStore.BLL.DTO;
using EcoStore.BLL.Services.Exceptions;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.Presentation.Mapping;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EcoStore.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly int _defaultPageNumber;
    private readonly int _defaultPageSize;
    private readonly string _imagePath;

    public HomeController(IProductService productService, IConfiguration configuration)
    {
        _productService = productService;
        _imagePath = configuration["Path:Images"]!;
        _defaultPageNumber = int.Parse(configuration["Defaults:PageNumber"]!, CultureInfo.InvariantCulture);
        _defaultPageSize = int.Parse(configuration["Defaults:PageSize"]!, CultureInfo.InvariantCulture);
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetProductsAsync(new ProductsFilterDTO
        {
            PageNumber = _defaultPageNumber,
            PageSize = _defaultPageSize,
            SortBy = SortByDTO.DateCreated,
            Descending = true,
            OnlyAvailable = true,
        });
        var productViewModels = products.Select(p => p.ToViewModel()).ToList();
        foreach (var product in productViewModels)
        {
            product.ImagePath = Path.Combine(_imagePath, product.ImagePath);
        }

        return View(productViewModels);
    }

    public IActionResult Delivery()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Error()
    {
        var exceptionHandler = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        return exceptionHandler?.Error switch
        {
            null => RedirectToAction("Index"),
            ObjectNotFoundException => View("NotFound"),
            ValidationException validationException => View("ValidationError", validationException.Errors.Select(e => e.Message)),
            _ => View(),
        };
    }
}