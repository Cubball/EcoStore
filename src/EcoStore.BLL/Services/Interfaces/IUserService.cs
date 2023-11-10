using EcoStore.BLL.DTO;

namespace EcoStore.BLL.Services.Interfaces;

public interface IUserService
{
    Task<bool> RegisterUserAsync(UserRegisterDTO registerDTO);

    Task<bool> RegisterAdminAsync(AdminRegisterDTO registerDTO);

    Task<bool> ChangePasswordAsync(UserChangePasswordDTO changePasswordDTO);

    Task<IEnumerable<AppUserDTO>> GetUsersAsync(int? pageNumber = null, int? pageSize = null);

    Task<int> GetUsersCountAsync();

    Task<AppUserDTO> GetUserByIdAsync(string id);

    Task UpdateUserAsync(UpdateAppUserDTO userDTO);

    Task DeleteUserAsync(string id);
}