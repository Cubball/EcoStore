using System.ComponentModel.DataAnnotations;
using EcoStore.BLL.DTO;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.BLL.Validation.Interfaces;

namespace EcoStore.BLL.Validation;

public class UserLoginValidator : IValidator<UserLoginDTO>
{
    private readonly EmailAddressAttribute _emailAddressAttribute = new();

    public Task ValidateAsync(UserLoginDTO obj)
    {
        var errors = new List<ValidationError>();
        if (string.IsNullOrWhiteSpace(obj.Email))
        {
            errors.Add(new ValidationError(nameof(obj.Email), "Пошта не може бути порожньою"));
        }
        else if (!_emailAddressAttribute.IsValid(obj.Email))
        {
            errors.Add(new ValidationError(nameof(obj.Email), "Введена пошта некоректна"));
        }

        if (string.IsNullOrWhiteSpace(obj.Password))
        {
            errors.Add(new ValidationError(nameof(obj.Password), "Пароль не може бути порожнім"));
        }
        else if (obj.Password.Length < 8)
        {
            errors.Add(new ValidationError(nameof(obj.Password), "Пароль повинен містити не менше 8 символів"));
        }

        return errors.Count > 0
            ? throw new Exceptions.ValidationException(errors)
            : Task.CompletedTask;
    }
}