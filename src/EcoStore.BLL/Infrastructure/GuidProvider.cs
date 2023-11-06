namespace EcoStore.BLL.Infrastructure;

public class GuidProvider :IGuidProvider
{
    public Guid NewGuid() => Guid.NewGuid();
}