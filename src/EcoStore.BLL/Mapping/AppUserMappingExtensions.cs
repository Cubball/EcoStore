using EcoStore.BLL.DTO;
using EcoStore.DAL.Entities;

namespace EcoStore.BLL.Mapping;

public static class AppUserMappingExtensions
{
    public static AppUserDTO ToDTO(this AppUser user)
    {
        return new AppUserDTO
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber!,
        };
    }

    public static AppUser ToEntity(this AdminRegisterDTO userDTO)
    {
        return new AppUser
        {
            Email = userDTO.Email,
        };
    }

    public static AppUser ToEntity(this UserRegisterDTO userDTO)
    {
        return new AppUser
        {
            Email = userDTO.Email,
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName,
            PhoneNumber = userDTO.PhoneNumber,
        };
    }
}