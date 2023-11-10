using EcoStore.BLL.DTO;
using EcoStore.BLL.Mapping;
using EcoStore.BLL.Services.Exceptions;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.BLL.Validation.Interfaces;
using EcoStore.DAL.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcoStore.BLL.Services;

public class UserService : IUserService
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 25;
    private readonly UserManager<AppUser> _userManager;
    private readonly IValidator<UserRegisterDTO> _userRegisterValidator;
    private readonly IValidator<AdminRegisterDTO> _adminRegisterValidator;
    private readonly IValidator<UserChangePasswordDTO> _userChangePasswordValidator;
    private readonly IValidator<UpdateAppUserDTO> _updateAppUserValidator;
    private readonly string _adminRoleName = Role.Admin.ToString();

    public UserService(UserManager<AppUser> userManager,
            IValidator<UserRegisterDTO> userRegisterValidator,
            IValidator<AdminRegisterDTO> adminRegisterValidator,
            IValidator<UserChangePasswordDTO> userChangePasswordValidator,
            IValidator<UpdateAppUserDTO> updateAppUserValidator)
    {
        _userManager = userManager;
        _userRegisterValidator = userRegisterValidator;
        _adminRegisterValidator = adminRegisterValidator;
        _userChangePasswordValidator = userChangePasswordValidator;
        _updateAppUserValidator = updateAppUserValidator;
    }

    public async Task<bool> ChangePasswordAsync(UserChangePasswordDTO changePasswordDTO)
    {
        await _userChangePasswordValidator.ValidateAsync(changePasswordDTO);
        var user = await TryGetUserByEmail(changePasswordDTO.Email);
        var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.OldPassword, changePasswordDTO.NewPassword);
        return result.Succeeded;
    }

    public async Task DeleteUserAsync(string id)
    {
        var user = await TryGetUserById(id);
        await _userManager.DeleteAsync(user);
    }

    public async Task<AppUserDTO> GetUserByIdAsync(string id)
    {
        var user = await TryGetUserById(id);
        var userDTO = user.ToDTO();
        userDTO.Role = await _userManager.IsInRoleAsync(user, _adminRoleName) ? RoleDTO.Admin : RoleDTO.User;
        return userDTO;
    }

    public async Task<IEnumerable<AppUserDTO>> GetUsersAsync(int? pageNumber = null, int? pageSize = null)
    {
        if (pageNumber is null or < 1)
        {
            pageNumber = DefaultPageNumber;
        }

        if (pageSize is null or < 1)
        {
            pageSize = DefaultPageSize;
        }

        var skip = (pageNumber - 1) * pageSize;
        var users = await _userManager.Users
            .Skip(skip.Value)
            .Take(pageSize.Value)
            .ToListAsync();
        var userDTOs = new List<AppUserDTO>(users.Count);
        foreach (var user in users)
        {
            var userDTO = user.ToDTO();
            userDTO.Role = await _userManager.IsInRoleAsync(user, _adminRoleName) ? RoleDTO.Admin : RoleDTO.User;
            userDTOs.Add(userDTO);
        }

        return userDTOs;
    }

    public async Task<int> GetUsersCountAsync()
    {
        return await _userManager.Users.CountAsync();
    }

    public async Task<bool> RegisterAdminAsync(AdminRegisterDTO registerDTO)
    {
        await _adminRegisterValidator.ValidateAsync(registerDTO);
        var user = registerDTO.ToEntity();
        user.UserName = registerDTO.Email;
        user.FirstName = registerDTO.Email;
        user.LastName = string.Empty;
        var result = await _userManager.CreateAsync(user, registerDTO.Password);
        if (!result.Succeeded)
        {
            return false;
        }

        result = await _userManager.AddToRoleAsync(user, _adminRoleName);
        return result.Succeeded;
    }

    public async Task<bool> RegisterUserAsync(UserRegisterDTO registerDTO)
    {
        await _userRegisterValidator.ValidateAsync(registerDTO);
        var user = registerDTO.ToEntity();
        user.UserName = registerDTO.Email;
        var result = await _userManager.CreateAsync(user, registerDTO.Password);
        return result.Succeeded;
    }

    public async Task UpdateUserAsync(UpdateAppUserDTO userDTO)
    {
        await _updateAppUserValidator.ValidateAsync(userDTO);
        var user = await TryGetUserByEmail(userDTO.Email);
        user.FirstName = userDTO.FirstName;
        user.LastName = userDTO.LastName;
        user.PhoneNumber = userDTO.PhoneNumber;
        await _userManager.UpdateAsync(user);
    }

    private async Task<AppUser> TryGetUserById(string id)
    {
        return await _userManager.FindByIdAsync(id)
            ?? throw new ObjectNotFoundException($"Користувача з Id {id} не було знайдено");
    }

    private async Task<AppUser> TryGetUserByEmail(string email)
    {
        return await _userManager.FindByEmailAsync(email)
            ?? throw new ObjectNotFoundException($"Користувача з поштою {email} не було знайдено");
    }
}