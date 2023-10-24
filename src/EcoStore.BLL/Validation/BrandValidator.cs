using EcoStore.BLL.DTO;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.BLL.Validators.Interfaces;

namespace EcoStore.BLL.Validation;

public class BrandValidator : IValidator<BrandDTO>
{
    public Task ValidateAsync(BrandDTO obj)
    {
        var errors = new List<ValidationError>();
        if (string.IsNullOrWhiteSpace(obj.Name))
        {
            errors.Add(new ValidationError(nameof(obj.Name), "Назва бренду не може бути порожньою"));
        }
        else if (obj.Name.Length is < 2 or > 100)
        {
            errors.Add(new ValidationError(nameof(obj.Name), "Назва бренду має містити від 2 до 100 символів"));
        }

        if (string.IsNullOrWhiteSpace(obj.Description))
        {
            errors.Add(new ValidationError(nameof(obj.Description), "Опис бренду не може бути порожнім"));
        }
        else if (obj.Description.Length is < 20 or > 1000)
        {
            errors.Add(new ValidationError(nameof(obj.Description), "Опис бренду має містити від 20 до 1000 символів"));
        }

        return errors.Any() ? throw new ValidationException(errors) : Task.CompletedTask;
    }
}