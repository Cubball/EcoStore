using EcoStore.BLL.DTO;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.BLL.Validation.Interfaces;
using EcoStore.DAL.Repositories.Interfaces;

namespace EcoStore.BLL.Validation;

public class ProductValidator : IValidator<CreateUpdateProductDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductValidator(
            IProductRepository productRepository,
            IBrandRepository brandRepository,
            ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _brandRepository = brandRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task ValidateAsync(CreateUpdateProductDTO obj)
    {
        var errors = new List<ValidationError>();
        if (string.IsNullOrWhiteSpace(obj.Name))
        {
            errors.Add(new ValidationError(nameof(obj.Name), "Назва товару не може бути порожньою"));
        }
        else if (obj.Name.Length is < 2 or > 100)
        {
            errors.Add(new ValidationError(nameof(obj.Name), "Назва товару має містити від 2 до 100 символів"));
        }

        if (await _productRepository.ProductExistsAsync(obj.Name))
        {
            errors.Add(new ValidationError(nameof(obj.Name), $"Товар з назвою {obj.Name} вже існує"));
        }

        if (string.IsNullOrWhiteSpace(obj.Description))
        {
            errors.Add(new ValidationError(nameof(obj.Description), "Опис товару не може бути порожнім"));
        }
        else if (obj.Description.Length is < 20 or > 1000)
        {
            errors.Add(new ValidationError(nameof(obj.Description), "Опис товару має містити від 20 до 1000 символів"));
        }

        if (obj.Price <= 0)
        {
            errors.Add(new ValidationError(nameof(obj.Price), "Ціна товару має бути більшою за 0"));
        }

        if (obj.Stock < 0)
        {
            errors.Add(new ValidationError(nameof(obj.Stock), "Кількість товару на складі не може бути від'ємною"));
        }

        if (!await _brandRepository.BrandExistsAsync(obj.BrandId))
        {
            errors.Add(new ValidationError(nameof(obj.BrandId), $"Бренд з id {obj.BrandId} не існує"));
        }

        if (!await _categoryRepository.CategoryExistsAsync(obj.CategoryId))
        {
            errors.Add(new ValidationError(nameof(obj.CategoryId), $"Категорія з id {obj.CategoryId} не існує"));
        }

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}