using EcoStore.BLL.DTO;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.BLL.Validation.Interfaces;
using EcoStore.DAL.Entities;
using EcoStore.DAL.Repositories.Interfaces;

namespace EcoStore.BLL.Validation;

public class CancelOrderByUserValidator : IValidator<CancelOrderByUserDTO>
{
    private readonly IOrderRepository _orderRepository;

    public CancelOrderByUserValidator(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task ValidateAsync(CancelOrderByUserDTO obj)
    {
        var errors = new List<ValidationError>();
        if (obj.Id <= 0)
        {
            errors.Add(new ValidationError(nameof(obj.Id), "Id замовлення має бути більше 0"));
        }
        else
        {
            var order = await _orderRepository.GetOrderByIdAsync(obj.Id);
            if (order.OrderStatus is not OrderStatus.New)
            {
                errors.Add(new ValidationError(nameof(obj.Id), "Замовлення не може бути скасоване"));
            }
        }

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
    }
}