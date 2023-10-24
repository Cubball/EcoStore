using EcoStore.BLL.DTO;
using EcoStore.DAL.Entities;

namespace EcoStore.BLL.Mapping;

public static class BrandMappingExtensions
{
    public static BrandDTO ToDTO(this Brand brand)
    {
        return new BrandDTO
        {
            Id = brand.Id,
            Name = brand.Name,
            Description = brand.Description
        };
    }

    public static Brand ToEntity(this BrandDTO brandDto)
    {
        return new Brand
        {
            Id = brandDto.Id,
            Name = brandDto.Name,
            Description = brandDto.Description
        };
    }
}