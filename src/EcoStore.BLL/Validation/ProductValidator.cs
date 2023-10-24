using EcoStore.BLL.DTO;
using EcoStore.BLL.Validation.Exceptions;
using EcoStore.BLL.Validation.Interfaces;

namespace EcoStore.BLL.Validation;

public class ProductValidator : IValidator<CreateUpdateProductDTO>
{
    public Task ValidateAsync(CreateUpdateProductDTO obj)
    {
        var errors = new List<ValidationError>();
        if (string.IsNullOrWhiteSpace(obj.Name))
        {
            errors.Add(new ValidationError(nameof(obj.Name), "Назва товару не може бути порожньою"));
        }
        else if (obj.Name.Length is < 2 or > 100)
        {
            errors.Add(new ValidationError(nameof(obj.Name), "Назва товару має містити від 2 до 100 символів"));
        }

        if (string.IsNullOrWhiteSpace(obj.Description))
        {
            errors.Add(new ValidationError(nameof(obj.Description), "Опис товару не може бути порожнім"));
        }
        else if (obj.Description.Length is < 20 or > 1000)
        {
            errors.Add(new ValidationError(nameof(obj.Description), "Опис товару має містити від 20 до 1000 символів"));
        }

        // TODO: Validate foreign keys??? Add corresponding methods to the repo
        // TODO: Make image nullable???

        if (obj.Price <= 0)
        {
            errors.Add(new ValidationError(nameof(obj.Price), "Ціна товару має бути більшою за 0"));
        }

        if (obj.Stock < 0)
        {
            errors.Add(new ValidationError(nameof(obj.Stock), "Кількість товару на складі не може бути від'ємною"));
        }

        return Task.CompletedTask;
    }
}
