using EcoStore.DAL.Entities;

namespace EcoStore.DAL.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task<string> AddPaymentAsync(Payment payment);

    Task DeletePaymentAsync(string id);
}