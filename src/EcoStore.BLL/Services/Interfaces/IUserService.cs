using EcoStore.BLL.DTO;

namespace EcoStore.BLL.Services.Interfaces;

public interface IUserService
{
    Task<bool> RegisterUserAsync(UserRegisterDTO registerDTO);

    Task<bool> RegisterAdminAsync(AdminRegisterDTO registerDTO);

    Task<bool> ChangePasswordAsync(UserChangePasswordDTO changePasswordDTO);

    Task<IEnumerable<AppUserDTO>> GetUsersAsync(int? pageNumber = null, int? pageSize = null, string? nameOrEmailSearchTerm = null);

    Task<int> GetUsersCountAsync(string? nameOrEmailSearchTerm = null);

    Task<AppUserDTO> GetUserByIdAsync(string id);

    Task UpdateUserAsync(UpdateAppUserDTO userDTO);

    Task DeleteUserAsync(string id);
}