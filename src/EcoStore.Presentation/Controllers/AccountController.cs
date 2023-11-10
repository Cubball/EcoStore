using EcoStore.BLL.Services.Interfaces;
using EcoStore.DAL.Entities;
using EcoStore.Presentation.Mapping;
using EcoStore.Presentation.ViewModels;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcoStore.Presentation.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(
            IUserService userService,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
    {
        _userService = userService;
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

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    // TODO: catch
    public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
    {
        var success = await _userService.RegisterUserAsync(signUpViewModel.ToDTO());
        if (!success)
        {
            // TODO: change this and the same code in Login
            ModelState.AddModelError(string.Empty, "Введені дані некоректні");
            return View(signUpViewModel);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Info()
    {
        var userId = _userManager.GetUserId(User);
        var userDTO = await _userService.GetUserByIdAsync(userId!);
        var userViewModel = userDTO.ToViewModel();
        return View(userViewModel);
    }

    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
    {
        var changePasswordDTO = changePasswordViewModel.ToDTO();
        changePasswordDTO.Id = _userManager.GetUserId(User)!;
        // note: doesnt throw, returns a bool
        var success = await _userService.ChangePasswordAsync(changePasswordDTO);
        if (!success)
        {
            return View();
        }
        // TODO: redirect to success page??
        return RedirectToAction("Profile");
    }

    public async Task<IActionResult> Update()
    {
        var userId = _userManager.GetUserId(User);
        var user = await _userService.GetUserByIdAsync(userId!);
        return View(user.ToUpdateViewModel());
    }

    // catch
    [HttpPost]
    public async Task<IActionResult> Update(UpdateAppUserViewModel updateAppUserViewModel)
    {
        var updateAppUserDTO = updateAppUserViewModel.ToDTO();
        await _userService.UpdateUserAsync(updateAppUserDTO);
        return RedirectToAction(nameof(Info));
    }
}