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
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly int _defaultPageNumber;
    private readonly int _defaultPageSize;

    public UsersController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _defaultPageNumber = int.Parse(configuration["Defaults:PageNumber"]!, CultureInfo.InvariantCulture);
        _defaultPageSize = int.Parse(configuration["Defaults:PageSize"]!, CultureInfo.InvariantCulture);
    }

    public async Task<IActionResult> All(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] string? search)
    {
        if (page < 1)
        {
            page = _defaultPageNumber;
        }

        if (pageSize < 1)
        {
            pageSize = _defaultPageSize;
        }

        var users = (await _userService.GetUsersAsync(page, pageSize, search))
            .Select(u => u.ToViewModel())
            .ToList();
        var usersCount = await _userService.GetUsersCountAsync(search);
        var usersListViewModel = new AppUsersListViewModel
        {
            PageInfo = new PageInfoViewModel
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = usersCount,
            },
            Users = users,
            Search = search,
        };
        return View(usersListViewModel);
    }

    public async Task<IActionResult> Details(string id)
    {
        var user = (await _userService.GetUserByIdAsync(id)).ToViewModel();
        return View(user);
    }

    public IActionResult RegisterAdmin()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAdmin(RegisterAdminViewModel registerAdmin)
    {
        var registerDTO = registerAdmin.ToDTO();
        try
        {
            await _userService.RegisterAdminAsync(registerDTO);
            return RedirectToAction(nameof(All));
        }
        catch (ValidationException e)
        {
            e.AddErrorsToModelState(ModelState);
            return View(registerAdmin);
        }
    }

    public async Task<IActionResult> Delete(string id)
    {
        var user = (await _userService.GetUserByIdAsync(id)).ToViewModel();
        return View(user);
    }

    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        await _userService.DeleteUserAsync(id);
        return RedirectToAction(nameof(All));
    }
}