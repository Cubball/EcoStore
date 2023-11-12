using System.Globalization;

using EcoStore.BLL.DTO;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.Presentation.Areas.Admin.Mapping;
using EcoStore.Presentation.Areas.Admin.ViewModels;
using EcoStore.Presentation.Mapping;
using EcoStore.Presentation.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoStore.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
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
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] SortProductsByViewModel sortBy,
        [FromQuery] string? search = null)
    {
        var filter = GetProductsFilter(page, pageSize, search, sortBy);
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

    public IActionResult Create()
    {
        return View();
    }

    // TODO : catch
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductViewModel createProduct)
    {
        var createProductDTO = createProduct.ToDTO();
        createProductDTO.ImageExtension = Path.GetExtension(createProduct.Image.FileName);
        createProductDTO.ImageStream = createProduct.Image.OpenReadStream();
        var productId = await _productService.CreateProductAsync(createProductDTO);
        return RedirectToAction(nameof(Details), new { id = productId });
    }

    public async Task<IActionResult> Update(int id)
    {
        var product = (await _productService.GetProductByIdAsync(id)).ToUpdateViewModel();
        return View(product);
    }

    // TODO : catch
    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductViewModel updateProduct)
    {
        var updateProductDTO = updateProduct.ToDTO();
        if (updateProduct.Image != null)
        {
            updateProductDTO.ImageExtension = Path.GetExtension(updateProduct.Image.FileName);
            updateProductDTO.ImageStream = updateProduct.Image.OpenReadStream();
        }

        await _productService.UpdateProductAsync(updateProductDTO);
        return RedirectToAction(nameof(Details), new { id = updateProduct.Id });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = (await _productService.GetProductByIdAsync(id)).ToViewModel();
        product.ImagePath = Path.Combine(_imagePath, product.ImagePath);
        return View(product);
    }

    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _productService.DeleteProductAsync(id);
        return RedirectToAction(nameof(All));
    }

    private ProductsFilterDTO GetProductsFilter(
            int page,
            int pageSize,
            string? search,
            SortProductsByViewModel sortBy)
    {
        if (page < 1)
        {
            page = _defaultPageNumber;
        }

        if (pageSize < 1)
        {
            pageSize = _defaultPageSize;
        }

        var (sortByDTO, descending) = sortBy.ToDTO();
        return new ProductsFilterDTO
        {
            PageNumber = page,
            PageSize = pageSize,
            SearchString = search,
            SortBy = sortByDTO,
            Descending = descending
        };
    }
}