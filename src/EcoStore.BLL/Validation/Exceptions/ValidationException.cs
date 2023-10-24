namespace EcoStore.BLL.Validation.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(IEnumerable<ValidationError> errors) : this("Отримані дані некоректні", errors)
    {
    }

    public ValidationException(string message, IEnumerable<ValidationError> errors) : base(message)
    {
        Errors = errors;
    }

    public IEnumerable<ValidationError> Errors { get; }
}