namespace EcoStore.DAL.Files.Exceptions;

public class FileNotFoundException : Exception
{
    public FileNotFoundException()
    {
    }

    public FileNotFoundException(string message)
        : base(message)
    {
    }

    public FileNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}