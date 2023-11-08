using EcoStore.BLL.DTO;
using EcoStore.Presentation.ViewModels;

namespace EcoStore.Presentation.Mapping;

public static class CategoryMappingExtensions
{
    public static CategoryViewModel ToViewModel(this CategoryDTO categoryDTO)
    {
        return new CategoryViewModel
        {
            Id = categoryDTO.Id,
            Name = categoryDTO.Name,
            Description = categoryDTO.Description,
        };
    }
}