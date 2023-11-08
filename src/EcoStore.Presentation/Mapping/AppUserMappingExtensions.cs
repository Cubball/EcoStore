using EcoStore.BLL.DTO;
using EcoStore.Presentation.ViewModels;

namespace EcoStore.Presentation.Mapping;

public static class AppUserMappingExtensions
{
    public static AppUserViewModel ToViewModel(this AppUserDTO appUserDTO)
    {
        return new AppUserViewModel
        {
            Id = appUserDTO.Id,
            Email = appUserDTO.Email,
            FirstName = appUserDTO.FirstName,
            LastName = appUserDTO.LastName,
            PhoneNumber = appUserDTO.PhoneNumber,
            Role = appUserDTO.Role.ToViewModel(),
        };
    }
}