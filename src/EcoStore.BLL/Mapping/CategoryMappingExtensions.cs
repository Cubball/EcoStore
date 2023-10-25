using EcoStore.BLL.DTO;
using EcoStore.DAL.Entities;

namespace EcoStore.BLL.Mapping;

public static class CategoryMappingExtensions
{
    public static CategoryDTO ToDTO(this Category category)
    {
        return new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public static Category ToEntity(this CreateCategoryDTO categoryDto)
    {
        return new Category
        {
            Name = categoryDto.Name,
            Description = categoryDto.Description
        };
    }

    public static Category ToEntity(this UpdateCategoryDTO categoryDto)
    {
        return new Category
        {
            Id = categoryDto.Id,
            Name = categoryDto.Name,
            Description = categoryDto.Description
        };
    }
}