using System.ComponentModel.DataAnnotations;
using EcoStore.BLL.DTO;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.BLL.Validation.Interfaces;

namespace EcoStore.BLL.Validation;

public class UpdateAppUserValidator : IValidator<UpdateAppUserDTO>
{
    private readonly PhoneAttribute _phoneAttribute = new();

    public Task ValidateAsync(UpdateAppUserDTO obj)
    {
        var errors = new List<ValidationError>();
        if (string.IsNullOrWhiteSpace(obj.FirstName))
        {
            errors.Add(new ValidationError(nameof(obj.FirstName), "Ім'я не може бути порожнім"));
        }
        else if (obj.FirstName.Length is < 2 or > 50)
        {
            errors.Add(new ValidationError(nameof(obj.FirstName), "Ім'я повинно містити від 2 до 50 символів"));
        }

        if (string.IsNullOrWhiteSpace(obj.LastName))
        {
            errors.Add(new ValidationError(nameof(obj.LastName), "Прізвище не може бути порожнім"));
        }
        else if (obj.LastName.Length is < 2 or > 50)
        {
            errors.Add(new ValidationError(nameof(obj.LastName), "Прізвище повинно містити від 2 до 50 символів"));
        }

        if (string.IsNullOrWhiteSpace(obj.PhoneNumber))
        {
            errors.Add(new ValidationError(nameof(obj.PhoneNumber), "Номер телефону не може бути порожнім"));
        }
        else if (!_phoneAttribute.IsValid(obj.PhoneNumber))
        {
            errors.Add(new ValidationError(nameof(obj.PhoneNumber), "Введений номер телефону некоректний"));
        }

        return errors.Count > 0
            ? throw new Exceptions.ValidationException(errors)
            : Task.CompletedTask;
    }
}