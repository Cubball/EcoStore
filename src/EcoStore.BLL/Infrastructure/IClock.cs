namespace EcoStore.BLL.Infrastructure;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}