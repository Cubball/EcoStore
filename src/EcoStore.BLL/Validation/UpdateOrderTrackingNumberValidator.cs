using EcoStore.BLL.DTO;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.BLL.Validation.Interfaces;

namespace EcoStore.BLL.Validation;

public class UpdateOrderTrackingNumberValidator : IValidator<UpdateOrderTrackingNumberDTO>
{
    public Task ValidateAsync(UpdateOrderTrackingNumberDTO obj)
    {
        var errors = new List<ValidationError>();
        if (obj.Id <= 0)
        {
            errors.Add(new ValidationError(nameof(obj.Id), "Id не може бути меншим або дорівнювати 0"));
        }

        if (string.IsNullOrWhiteSpace(obj.TrackingNumber))
        {
            errors.Add(new ValidationError(nameof(obj.TrackingNumber), "Накладна не може бути порожньою"));
        }
        else if (obj.TrackingNumber.Length > 50)
        {
            errors.Add(new ValidationError(nameof(obj.TrackingNumber), "Накладна не може бути більшою за 50 символів"));
        }

        return errors.Count > 0
            ? throw new ValidationException(errors)
            : Task.CompletedTask;
    }
}