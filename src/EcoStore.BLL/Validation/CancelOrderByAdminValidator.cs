using EcoStore.BLL.DTO;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.BLL.Validation.Interfaces;
using EcoStore.DAL.Entities;
using EcoStore.DAL.Repositories.Interfaces;

namespace EcoStore.BLL.Validation;

public class CancelOrderByAdminValidator : IValidator<CancelOrderByAdminDTO>
{
    private readonly IOrderRepository _orderRepository;

    public CancelOrderByAdminValidator(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task ValidateAsync(CancelOrderByAdminDTO obj)
    {
        var errors = new List<ValidationError>();
        if (obj.Id <= 0)
        {
            errors.Add(new ValidationError(nameof(obj.Id), "Id замовлення має бути більше 0"));
        }
        else
        {
            var order = await _orderRepository.GetOrderByIdAsync(obj.Id);
            if (order.OrderStatus is not OrderStatus.New and not OrderStatus.Processing)
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