using EcoStore.BLL.DTO;
using EcoStore.Presentation.Areas.Admin.ViewModels;

namespace EcoStore.Presentation.Areas.Admin.Mapping;

public static class AppUserMappingExtensions
{
    public static AdminRegisterDTO ToDTO(this RegisterAdminViewModel registerAdminViewModel)
    {
        return new AdminRegisterDTO
        {
            Email = registerAdminViewModel.Email,
            Password = registerAdminViewModel.Password,
            ConfirmPassword = registerAdminViewModel.ConfirmPassword,
        };
    }
}