namespace EcoStore.BLL.Services.Exceptions;

public class AdminCreationFailedException : Exception
{
    public AdminCreationFailedException()
    {
    }

    public AdminCreationFailedException(string message) : base(message)
    {
    }

    public AdminCreationFailedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}