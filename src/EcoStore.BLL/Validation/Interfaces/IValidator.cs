namespace EcoStore.BLL.Validation.Interfaces;

public interface IValidator<T>
{
    Task ValidateAsync(T obj);
}