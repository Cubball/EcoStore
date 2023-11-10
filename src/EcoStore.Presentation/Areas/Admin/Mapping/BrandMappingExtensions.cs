using EcoStore.BLL.DTO;
using EcoStore.Presentation.Areas.Admin.ViewModels;

namespace EcoStore.Presentation.Areas.Admin.Mapping;

public static class BrandMappingExtensions
{
    public static CreateBrandDTO ToDTO(this CreateBrandViewModel createBrandViewModel)
    {
        return new CreateBrandDTO
        {
            Name = createBrandViewModel.Name,
            Description = createBrandViewModel.Description,
        };
    }

    public static UpdateBrandViewModel ToUpdateViewModel(this BrandDTO brandDTO)
    {
        return new UpdateBrandViewModel
        {
            Id = brandDTO.Id,
            Name = brandDTO.Name,
            Description = brandDTO.Description,
        };
    }

    public static UpdateBrandDTO ToDTO(this UpdateBrandViewModel updateBrandViewModel)
    {
        return new UpdateBrandDTO
        {
            Id = updateBrandViewModel.Id,
            Name = updateBrandViewModel.Name,
            Description = updateBrandViewModel.Description,
        };
    }
}