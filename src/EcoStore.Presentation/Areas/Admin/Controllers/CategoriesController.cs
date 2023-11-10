using System.Globalization;

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
public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly int _defaultPageNumber;
    private readonly int _defaultPageSize;

    public CategoriesController(ICategoryService categoryService,
            IConfiguration configuration)
    {
        _categoryService = categoryService;
        _defaultPageNumber = int.Parse(configuration["Defaults:PageNumber"]!, CultureInfo.InvariantCulture);
        _defaultPageSize = int.Parse(configuration["Defaults:PageSize"]!, CultureInfo.InvariantCulture);
    }

    public async Task<IActionResult> All([FromQuery] int page, [FromQuery] int pageSize)
    {
        if (page < 1)
        {
            page = _defaultPageNumber;
        }

        if (pageSize < 1)
        {
            pageSize = _defaultPageSize;
        }

        var categories = (await _categoryService.GetCategoriesAsync(page, pageSize))
                .Select(b => b.ToViewModel())
                .ToList();
        var categoriesCount = await _categoryService.GetCategoriesCountAsync();
        var categoriesListViewModel = new CategoriesListViewModel
        {
            PageInfo = new PageInfoViewModel
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = categoriesCount,
            },
            Categories = categories,
        };
        return View(categoriesListViewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var category = (await _categoryService.GetCategoryAsync(id)).ToViewModel();
        return View(category);
    }

    public IActionResult Create()
    {
        return View();
    }

    // catch
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryViewModel createCategoryViewModel)
    {
        var createCategoryDTO = createCategoryViewModel.ToDTO();
        var categoryId = await _categoryService.CreateCategoryAsync(createCategoryDTO);
        return RedirectToAction(nameof(Details), new { id = categoryId });
    }

    public async Task<IActionResult> Update(int id)
    {
        var category = (await _categoryService.GetCategoryAsync(id)).ToUpdateViewModel();
        return View(category);
    }

    // catch
    [HttpPost]
    public async Task<IActionResult> Update(UpdateCategoryViewModel updateCategoryViewModel)
    {
        var updateCategoryDTO = updateCategoryViewModel.ToDTO();
        await _categoryService.UpdateCategoryAsync(updateCategoryDTO);
        return RedirectToAction(nameof(Details), new { id = updateCategoryDTO.Id });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var category = (await _categoryService.GetCategoryAsync(id)).ToViewModel();
        return View(category);
    }

    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        return RedirectToAction(nameof(All));
    }
}