using EcoStore.BLL.Services.Interfaces;
using EcoStore.DAL.Entities;
using EcoStore.Presentation.ViewModels;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcoStore.Presentation.Controllers;

public class AccountController : Controller
{
    // private readonly IUserService _userService;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(
            // IUserService userService,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
    {
        // _userService = userService;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(loginViewModel);
        }

        var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Введені дані некоректні");
            return View(loginViewModel);
        }

        var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Введені дані некоректні");
            return View(loginViewModel);
        }

        return RedirectToAction("Index", "Home");
    }
}