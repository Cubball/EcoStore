using EcoStore.BLL.DTO;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.BLL.Validation.Interfaces;

namespace EcoStore.BLL.Validation;

public class UpdateBrandValidator : IValidator<UpdateBrandDTO>
{
    public Task ValidateAsync(UpdateBrandDTO obj)
    {
        var errors = new List<ValidationError>();
        if (obj.Id <= 0)
        {
            errors.Add(new ValidationError(nameof(obj.Id), "Id бренду не може бути меншим або рівним 0"));
        }

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

        return errors.Count > 0
            ? throw new ValidationException(errors)
            : Task.CompletedTask;
    }
}