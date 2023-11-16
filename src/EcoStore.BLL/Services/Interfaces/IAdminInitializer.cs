namespace EcoStore.BLL.Services.Interfaces;

public interface IAdminInitializerService
{
    Task InitializeAsync(string email, string password);
}