using EcoStore.BLL.DTO;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.BLL.Validation.Interfaces;
using EcoStore.DAL.Entities;
using EcoStore.DAL.Repositories.Exceptions;
using EcoStore.DAL.Repositories.Interfaces;

namespace EcoStore.BLL.Validation;

public class UpdateOrderStatusValidator : IValidator<UpdateOrderStatusDTO>
{
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderStatusValidator(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task ValidateAsync(UpdateOrderStatusDTO obj)
    {
        var errors = new List<ValidationError>();
        if (obj.Id <= 0)
        {
            errors.Add(new ValidationError(nameof(obj.Id), "Id не може бути меншим або дорівнювати 0"));
        }
        else
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(obj.Id);
                if (order.OrderStatus is OrderStatus.CancelledByUser or OrderStatus.CancelledByAdmin)
                {
                    errors.Add(new ValidationError(nameof(obj.Id), $"Замовлення з Id {obj.Id} вже скасовано"));
                }
            }
            catch (EntityNotFoundException)
            {
                errors.Add(new ValidationError(nameof(obj.Id), $"Замовлення з Id {obj.Id} не існує"));
            }
        }

        if (!Enum.IsDefined(typeof(OrderStatus), obj.OrderStatus))
        {
            errors.Add(new ValidationError(nameof(obj.OrderStatus), $"Статус замовлення {obj.OrderStatus} не існує"));
        }
        else
        {
            var status = Enum.Parse<OrderStatus>(obj.OrderStatus);
            if (status is OrderStatus.CancelledByUser or OrderStatus.CancelledByAdmin)
            {
                errors.Add(new ValidationError(nameof(obj.OrderStatus), $"Статус замовлення {obj.OrderStatus} не може бути встановлений вручну"));
            }
        }

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
    }
}