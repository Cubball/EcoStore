using EcoStore.BLL.DTO;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.Presentation.Mapping;
using EcoStore.Presentation.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace EcoStore.Presentation.Controllers;

public class HomeController : Controller
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 25;
    private readonly IProductService _productService;
    private readonly string _imagePath;

    public HomeController(IProductService productService, IConfiguration configuration)
    {
        _productService = productService;
        _imagePath = configuration["Path:Images"]!;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetProductsAsync(new ProductsFilterDTO
        {
            PageNumber = DefaultPageNumber,
            PageSize = DefaultPageSize,
            SortBy = SortBy.DateCreated,
            Descending = true,
        });
        var productViewModels = new List<ProductViewModel>();
        foreach (var product in products)
        {
            var viewModel = product.ToViewModel();
            viewModel.ImagePath = Path.Combine(_imagePath, product.ImageName);
            productViewModels.Add(viewModel);
        }

        return View(productViewModels);
    }
}