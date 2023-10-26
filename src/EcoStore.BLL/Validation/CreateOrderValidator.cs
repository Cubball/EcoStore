using EcoStore.BLL.DTO;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.BLL.Validation.Interfaces;
using EcoStore.DAL.Entities;
using EcoStore.DAL.Repositories.Exceptions;
using EcoStore.DAL.Repositories.Interfaces;

using Microsoft.AspNetCore.Identity;

namespace EcoStore.BLL.Validation;

public class CreateOrderValidator : IValidator<CreateOrderDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly UserManager<AppUser> _userManager;

    public CreateOrderValidator(IProductRepository productRepository, UserManager<AppUser> userManager)
    {
        _productRepository = productRepository;
        _userManager = userManager;
    }

    public async Task ValidateAsync(CreateOrderDTO obj)
    {
        var errors = new List<ValidationError>();
        var userNotFound = (await _userManager.FindByIdAsync(obj.UserId)) is null;
        if (userNotFound)
        {
            errors.Add(new ValidationError(nameof(obj.UserId), $"Користувача з Id {obj.UserId} не знайдено"));
        }

        if (!Enum.IsDefined(typeof(PaymentMethod), obj.PaymentMethod))
        {
            errors.Add(new ValidationError(nameof(obj.PaymentMethod), $"Метод оплати {obj.PaymentMethod} не існує"));
        }
        else if (Enum.Parse<PaymentMethod>(obj.PaymentMethod) == PaymentMethod.Card &&
                 string.IsNullOrWhiteSpace(obj.StripeToken))
        {
            errors.Add(new ValidationError(nameof(obj.StripeToken), "Токен для оплати картою відсутній"));
        }

        if (string.IsNullOrWhiteSpace(obj.ShippingAddress))
        {
            errors.Add(new ValidationError(nameof(obj.ShippingAddress), "Адреса доставки не може бути порожньою"));
        }
        else if (obj.ShippingAddress.Length is < 10 or > 200)
        {
            errors.Add(new ValidationError(nameof(obj.ShippingAddress), "Адреса доставки має бути від 10 до 200 символів"));
        }

        if (!Enum.IsDefined(typeof(ShippingMethod), obj.ShippingMethod))
        {
            errors.Add(new ValidationError(nameof(obj.ShippingMethod), $"Метод доставки {obj.ShippingMethod} не існує"));
        }

        if (obj.OrderedProducts is null || !obj.OrderedProducts.Any())
        {
            errors.Add(new ValidationError(nameof(obj.OrderedProducts), "Замовлення не може бути порожнім"));
        }
        else
        {
            await ValidateOrderedProducts(obj, errors);
        }

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
    }

    private async Task ValidateOrderedProducts(CreateOrderDTO obj, List<ValidationError> errors)
    {
        foreach (var orderedProduct in obj.OrderedProducts)
        {
            if (orderedProduct.Quantity < 1)
            {
                errors.Add(new ValidationError(nameof(orderedProduct.Quantity), "Кількість товару не може бути меншою за 1"));
            }

            try
            {
                var product = await _productRepository.GetProductByIdAsync(orderedProduct.ProductId);
                if (orderedProduct.Quantity > product.Stock)
                {
                    errors.Add(new ValidationError(nameof(orderedProduct.Quantity), $"Кількість товару не може бути більшою за {product.Stock}"));
                }
            }
            catch (EntityNotFoundException)
            {
                errors.Add(new ValidationError(nameof(orderedProduct.ProductId), $"Товару з Id {orderedProduct.ProductId} не знайдено"));
            }
        }
    }
}