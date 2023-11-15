using System.Globalization;

using EcoStore.BLL.Services.Interfaces;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.Presentation.Areas.Admin.Mapping;
using EcoStore.Presentation.Areas.Admin.ViewModels;
using EcoStore.Presentation.Extensions;
using EcoStore.Presentation.Mapping;
using EcoStore.Presentation.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoStore.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class BrandsController : Controller
{
    private readonly IBrandService _brandService;
    private readonly int _defaultPageNumber;
    private readonly int _defaultPageSize;

    public BrandsController(IBrandService brandService,
            IConfiguration configuration)
    {
        _brandService = brandService;
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

        var brands = (await _brandService.GetBrandsAsync(page, pageSize))
                .Select(b => b.ToViewModel())
                .ToList();
        var brandsCount = await _brandService.GetBrandsCountAsync();
        var brandsListViewModel = new BrandsListViewModel
        {
            PageInfo = new PageInfoViewModel
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = brandsCount,
            },
            Brands = brands,
        };
        return View(brandsListViewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var brand = (await _brandService.GetBrandAsync(id)).ToViewModel();
        return View(brand);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBrandViewModel createBrandViewModel)
    {
        var createBrandDTO = createBrandViewModel.ToDTO();
        try
        {
            var brandId = await _brandService.CreateBrandAsync(createBrandDTO);
            return RedirectToAction(nameof(Details), new { id = brandId });
        }
        catch (ValidationException e)
        {
            e.AddErrorsToModelState(ModelState);
            return View(createBrandViewModel);
        }
    }

    public async Task<IActionResult> Update(int id)
    {
        var brand = (await _brandService.GetBrandAsync(id)).ToUpdateViewModel();
        return View(brand);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateBrandViewModel updateBrandViewModel)
    {
        var updateBrandDTO = updateBrandViewModel.ToDTO();
        try
        {
            await _brandService.UpdateBrandAsync(updateBrandDTO);
            return RedirectToAction(nameof(Details), new { id = updateBrandDTO.Id });
        }
        catch (ValidationException e)
        {
            e.AddErrorsToModelState(ModelState);
            return View(updateBrandViewModel);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var brand = (await _brandService.GetBrandAsync(id)).ToViewModel();
        return View(brand);
    }

    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _brandService.DeleteBrandAsync(id);
        return RedirectToAction(nameof(All));
    }
}