using EcoStore.BLL.DTO;
using EcoStore.Presentation.ViewModels;

namespace EcoStore.Presentation.Mapping;

public static class BrandMappingExtensions
{
    public static BrandViewModel ToViewModel(this BrandDTO brandDTO)
    {
        return new BrandViewModel
        {
            Id = brandDTO.Id,
            Name = brandDTO.Name,
            Description = brandDTO.Description,
        };
    }
}