using EcoStore.BLL.DTO;
using EcoStore.Presentation.ViewModels;

namespace EcoStore.Presentation.Mapping;

public static class ProductMappingExtensions
{
    public static ProductViewModel ToViewModel(this ProductDTO productDTO)
    {
        var viewModel = new ProductViewModel
        {
            Id = productDTO.Id,
            Name = productDTO.Name,
            Description = productDTO.Description,
            Price = productDTO.Price,
            Stock = productDTO.Stock,
        };
        if (productDTO.Category is not null)
        {
            viewModel.CategoryId = productDTO.Category.Id;
            viewModel.Category = productDTO.Category.Name;
        }

        if (productDTO.Brand is not null)
        {
            viewModel.BrandId = productDTO.Brand.Id;
            viewModel.Brand = productDTO.Brand.Name;
        }

        return viewModel;
    }
}