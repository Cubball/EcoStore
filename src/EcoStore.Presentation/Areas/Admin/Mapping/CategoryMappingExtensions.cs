using EcoStore.BLL.DTO;
using EcoStore.Presentation.Areas.Admin.ViewModels;

namespace EcoStore.Presentation.Areas.Admin.Mapping;

public static class CategoryMappingExtensions
{
    public static CreateCategoryDTO ToDTO(this CreateCategoryViewModel createCategoryViewModel)
    {
        return new CreateCategoryDTO
        {
            Name = createCategoryViewModel.Name,
            Description = createCategoryViewModel.Description,
        };
    }

    public static UpdateCategoryViewModel ToUpdateViewModel(this CategoryDTO categoryDTO)
    {
        return new UpdateCategoryViewModel
        {
            Id = categoryDTO.Id,
            Name = categoryDTO.Name,
            Description = categoryDTO.Description,
        };
    }

    public static UpdateCategoryDTO ToDTO(this UpdateCategoryViewModel updateCategoryViewModel)
    {
        return new UpdateCategoryDTO
        {
            Id = updateCategoryViewModel.Id,
            Name = updateCategoryViewModel.Name,
            Description = updateCategoryViewModel.Description,
        };
    }
}