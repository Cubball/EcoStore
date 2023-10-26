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
        };
    }
}