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

    public static UserChangePasswordDTO ToDTO(this ChangePasswordViewModel changePasswordViewModel)
    {
        return new UserChangePasswordDTO
        {
            OldPassword = changePasswordViewModel.OldPassword,
            NewPassword = changePasswordViewModel.NewPassword,
            ConfirmNewPassword = changePasswordViewModel.ConfirmNewPassword,
        };
    }

    public static UpdateAppUserViewModel ToUpdateViewModel(this AppUserDTO appUserDTO)
    {
        return new UpdateAppUserViewModel
        {
            Id = appUserDTO.Id,
            FirstName = appUserDTO.FirstName,
            LastName = appUserDTO.LastName,
            PhoneNumber = appUserDTO.PhoneNumber,
        };
    }

    public static UpdateAppUserDTO ToDTO(this UpdateAppUserViewModel updateAppUserViewModel)
    {
        return new UpdateAppUserDTO
        {
            Id = updateAppUserViewModel.Id,
            FirstName = updateAppUserViewModel.FirstName,
            LastName = updateAppUserViewModel.LastName,
            PhoneNumber = updateAppUserViewModel.PhoneNumber,
        };
    }
}