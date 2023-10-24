using EcoStore.BLL.DTO;
using EcoStore.DAL.Entities;

namespace EcoStore.BLL.Mapping;

public static class ProductMappingExtensions
{
    public static ProductDTO ToDTO(this Product product)
    {
        return new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Stock = product.Stock,
            Brand = product.Brand.ToDTO(),
            Category = product.Category.ToDTO()
        };
    }

    public static Product ToEntity(this CreateUpdateProductDTO productDTO)
    {
        return new Product
        {
            Name = productDTO.Name,
            Description = productDTO.Description,
            Price = productDTO.Price,
            ImageUrl = productDTO.ImageUrl,
            Stock = productDTO.Stock,
            BrandId = productDTO.BrandId,
            CategoryId = productDTO.CategoryId
        };
    }
}