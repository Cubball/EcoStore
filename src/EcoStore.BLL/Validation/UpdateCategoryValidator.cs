using EcoStore.BLL.DTO;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.BLL.Validation.Interfaces;

namespace EcoStore.BLL.Validation;

public class UpdateCategoryValidator : IValidator<UpdateCategoryDTO>
{
    public Task ValidateAsync(UpdateCategoryDTO obj)
    {
        var errors = new List<ValidationError>();
        if (obj.Id <= 0)
        {
            errors.Add(new ValidationError(nameof(obj.Id), "Id категорії не може бути меншим або рівним 0"));
        }

        if (string.IsNullOrWhiteSpace(obj.Name))
        {
            errors.Add(new ValidationError(nameof(obj.Name), "Назва категорії не може бути порожньою"));
        }
        else if (obj.Name.Length is < 2 or > 100)
        {
            errors.Add(new ValidationError(nameof(obj.Name), "Назва категорії має містити від 2 до 100 символів"));
        }

        if (string.IsNullOrWhiteSpace(obj.Description))
        {
            errors.Add(new ValidationError(nameof(obj.Description), "Опис категорії не може бути порожнім"));
        }
        else if (obj.Description.Length is < 20 or > 1000)
        {
            errors.Add(new ValidationError(nameof(obj.Description), "Опис категорії має містити від 20 до 1000 символів"));
        }

        return errors.Count > 0
            ? throw new ValidationException(errors)
            : Task.CompletedTask;
    }
}