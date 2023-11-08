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

    public static UserRegisterDTO ToDTO(this SignUpViewModel signUpViewModel)
    {
        return new UserRegisterDTO
        {
            Email = signUpViewModel.Email,
            FirstName = signUpViewModel.FirstName,
            LastName = signUpViewModel.LastName,
            Password = signUpViewModel.Password,
            ConfirmPassword = signUpViewModel.ConfirmPassword,
            PhoneNumber = signUpViewModel.PhoneNumber,
        };
    }
}