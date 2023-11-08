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
            ImagePath = productDTO.ImageName,
            Stock = productDTO.Stock,
            Category = productDTO.Category?.ToViewModel(),
            Brand = productDTO.Brand?.ToViewModel(),
        };

        return viewModel;
    }
}