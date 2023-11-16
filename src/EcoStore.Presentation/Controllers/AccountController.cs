using EcoStore.BLL.Services.Interfaces;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.DAL.Entities;
using EcoStore.Presentation.Extensions;
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
    public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
    {
        try
        {
            var success = await _userService.RegisterUserAsync(signUpViewModel.ToDTO());
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Введені дані некоректні");
                return View(signUpViewModel);
            }

            return RedirectToAction(nameof(Login));
        }
        catch (ValidationException e)
        {
            e.AddErrorsToModelState(ModelState);
            return View(signUpViewModel);
        }
    }

    public async Task<IActionResult> Index()
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
        try
        {
            var success = await _userService.ChangePasswordAsync(changePasswordDTO);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Не вдалося змінити пароль");
                return View(changePasswordViewModel);
            }
        }
        catch (ValidationException e)
        {
            e.AddErrorsToModelState(ModelState);
            return View(changePasswordViewModel);
        }

        return View("PasswordChanged");
    }

    public async Task<IActionResult> Update()
    {
        var userId = _userManager.GetUserId(User);
        var user = await _userService.GetUserByIdAsync(userId!);
        return View(user.ToUpdateViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateAppUserViewModel updateAppUserViewModel)
    {
        var updateAppUserDTO = updateAppUserViewModel.ToDTO();
        try
        {
            await _userService.UpdateUserAsync(updateAppUserDTO);
            return RedirectToAction(nameof(Index));
        }
        catch (ValidationException e)
        {
            e.AddErrorsToModelState(ModelState);
            return View(updateAppUserViewModel);
        }
    }

    public IActionResult AccessDenied()
    {
        return RedirectToAction("Index", "Home");
    }
}