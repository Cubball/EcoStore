using EcoStore.BLL.DTO;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.BLL.Validation.Interfaces;

namespace EcoStore.BLL.Validation;

public class UserChangePasswordValidator : IValidator<UserChangePasswordDTO>
{

    public Task ValidateAsync(UserChangePasswordDTO obj)
    {
        var errors = new List<ValidationError>();
        if (string.IsNullOrWhiteSpace(obj.OldPassword))
        {
            errors.Add(new ValidationError(nameof(obj.OldPassword), "Старий пароль не може бути порожнім"));
        }
        else if (obj.OldPassword.Length < 8)
        {
            errors.Add(new ValidationError(nameof(obj.OldPassword), "Старий пароль повинен містити не менше 8 символів"));
        }

        if (string.IsNullOrWhiteSpace(obj.NewPassword))
        {
            errors.Add(new ValidationError(nameof(obj.NewPassword), "Новий пароль не може бути порожнім"));
        }
        else if (obj.NewPassword.Length < 8)
        {
            errors.Add(new ValidationError(nameof(obj.NewPassword), "Новий пароль повинен містити не менше 8 символів"));
        }

        if (obj.NewPassword != obj.ConfirmNewPassword)
        {
            errors.Add(new ValidationError(nameof(obj.ConfirmNewPassword), "Паролі не співпадають"));
        }

        return errors.Count > 0
            ? throw new ValidationException(errors)
            : Task.CompletedTask;
    }
}