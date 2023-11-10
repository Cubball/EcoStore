using EcoStore.BLL.DTO;
using EcoStore.Presentation.Areas.Admin.ViewModels;

namespace EcoStore.Presentation.Areas.Admin.Mapping;

public static class ProductMappingExtensions
{
    public static CreateProductDTO ToDTO(this CreateProductViewModel createProductViewModel)
    {
        return new CreateProductDTO
        {
            Name = createProductViewModel.Name,
            Description = createProductViewModel.Description,
            Price = createProductViewModel.Price,
            Stock = createProductViewModel.Stock,
            CategoryId = createProductViewModel.CategoryId,
            BrandId = createProductViewModel.BrandId,
        };
    }

    public static UpdateProductViewModel ToUpdateViewModel(this ProductDTO productDTO)
    {
        return new UpdateProductViewModel
        {
            Id = productDTO.Id,
            Name = productDTO.Name,
            Description = productDTO.Description,
            Price = productDTO.Price,
            Stock = productDTO.Stock,
            CategoryId = productDTO.Category!.Id,
            BrandId = productDTO.Brand!.Id,
        };
    }

    public static UpdateProductDTO ToDTO(this UpdateProductViewModel updateProductViewModel)
    {
        return new UpdateProductDTO
        {
            Id = updateProductViewModel.Id,
            Name = updateProductViewModel.Name,
            Description = updateProductViewModel.Description,
            Price = updateProductViewModel.Price,
            Stock = updateProductViewModel.Stock,
            CategoryId = updateProductViewModel.CategoryId,
            BrandId = updateProductViewModel.BrandId,
        };
    }
}