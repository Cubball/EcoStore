namespace EcoStore.DAL.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task DeletePaymentAsync(string id);
}