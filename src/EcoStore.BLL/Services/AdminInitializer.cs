using EcoStore.BLL.DTO;
using EcoStore.BLL.Services.Exceptions;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.DAL.Entities;

using Microsoft.AspNetCore.Identity;

namespace EcoStore.BLL.Services;

public class AdminInitializerService : IAdminInitializerService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminInitializerService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync(string email, string password)
    {
        var adminRoleName = RoleDTO.Admin.ToString();
        if (!await _roleManager.RoleExistsAsync(adminRoleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(adminRoleName));
        }

        var admins = await _userManager.GetUsersInRoleAsync(adminRoleName);
        if (admins.Count > 0)
        {
            return;
        }

        var admin = new AppUser
        {
            UserName = email,
            Email = email,
            FirstName = adminRoleName,
            LastName = string.Empty,
        };
        var result = await _userManager.CreateAsync(admin, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(';', result.Errors.Select(e => e.Description));
            throw new AdminCreationFailedException($"Не вдалося створити аккаунт адміністратора. Помилки: {errors}");
        }

        result = await _userManager.AddToRoleAsync(admin, adminRoleName);
        if (!result.Succeeded)
        {
            var errors = string.Join(';', result.Errors.Select(e => e.Description));
            throw new AdminCreationFailedException($"Не вдалося надати новому аккаунту роль адміністратора. Помилки: {errors}");
        }
    }
}