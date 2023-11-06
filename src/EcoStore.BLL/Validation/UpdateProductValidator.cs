using EcoStore.BLL.DTO;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.BLL.Validation.Interfaces;
using EcoStore.DAL.Repositories.Interfaces;

namespace EcoStore.BLL.Validation;

public class UpdateProductValidator : IValidator<UpdateProductDTO>
{
    private readonly IBrandRepository _brandRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateProductValidator(
            IBrandRepository brandRepository,
            ICategoryRepository categoryRepository)
    {
        _brandRepository = brandRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task ValidateAsync(UpdateProductDTO obj)
    {
        var errors = new List<ValidationError>();
        if (obj.Id <= 0)
        {
            errors.Add(new ValidationError(nameof(obj.Id), "Id товару не може бути меншим або рівним 0"));
        }

        if (string.IsNullOrWhiteSpace(obj.Name))
        {
            errors.Add(new ValidationError(nameof(obj.Name), "Назва товару не може бути порожньою"));
        }
        else if (obj.Name.Length is < 2 or > 100)
        {
            errors.Add(new ValidationError(nameof(obj.Name), "Назва товару має містити від 2 до 100 символів"));
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

        if ((obj.ImageStream is null) != (obj.ImageExtension is null))
        {
            errors.Add(new ValidationError(nameof(obj.ImageStream), "Зображення та його розширення повинні обоє або бути присутні, або відсутні"));
        }

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
    }
}