using EcoStore.BLL.DTO;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.BLL.Validation.Interfaces;
using EcoStore.DAL.Entities;

namespace EcoStore.BLL.Validation;

public class UpdateOrderStatusValidator : IValidator<UpdateOrderStatusDTO>
{
    public Task ValidateAsync(UpdateOrderStatusDTO obj)
    {
        var errors = new List<ValidationError>();
        if (obj.Id <= 0)
        {
            errors.Add(new ValidationError(nameof(obj.Id), "Id не може бути меншим або дорівнювати 0"));
        }

        if (!Enum.IsDefined(typeof(OrderStatus), obj.OrderStatus))
        {
            errors.Add(new ValidationError(nameof(obj.OrderStatus), $"Статус замовлення {obj.OrderStatus} не існує"));
        }

        return errors.Any()
            ? throw new ValidationException(errors)
            : Task.CompletedTask;
    }
}