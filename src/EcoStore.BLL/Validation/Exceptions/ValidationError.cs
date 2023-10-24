namespace EcoStore.BLL.Validation.Exceptions;

public class ValidationError
{
    public ValidationError(string property, string message)
    {
        Property = property;
        Message = message;
    }

    public string Property { get; set; }

    public string Message { get; set; }
}