using EcoStore.DAL.EF;
using EcoStore.DAL.Entities;
using EcoStore.DAL.Repositories.Exceptions;
using EcoStore.DAL.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace EcoStore.DAL.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> AddPaymentAsync(Payment payment)
    {
        try
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment.Id;
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося додати платіж", e);
        }
    }

    public async Task DeletePaymentAsync(string id)
    {
        var payment = await GetPaymentByIdAsync(id);
        try
        {
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося видалити платіж", e);
        }
    }

    public async Task<Payment> GetPaymentByIdAsync(string id)
    {
        return await _context.Payments.FindAsync(id)
            ?? throw new EntityNotFoundException($"Платіж з Id {id} не знайдено");
    }

    public async Task UpdatePaymentAsync(Payment payment)
    {
        var retrievedPayment = await GetPaymentByIdAsync(payment.Id);
        try
        {
            UpdatePaymentProperties(retrievedPayment, payment);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new RepositoryException("Не вдалося оновити платіж", e);
        }
    }

    private static void UpdatePaymentProperties(Payment paymentFromDb, Payment newPayment)
    {
        paymentFromDb.Created = newPayment.Created;
        paymentFromDb.Amount = newPayment.Amount;
        paymentFromDb.Currency = newPayment.Currency;
    }
}
