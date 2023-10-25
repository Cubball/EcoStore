using EcoStore.DAL.Entities;

namespace EcoStore.DAL.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> GetPaymentByIdAsync(string id);

    Task<string> AddPaymentAsync(Payment payment);

    Task UpdatePaymentAsync(Payment payment);

    Task DeletePaymentAsync(string id);
}