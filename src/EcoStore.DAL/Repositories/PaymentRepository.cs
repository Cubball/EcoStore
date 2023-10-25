using EcoStore.DAL.EF;
using EcoStore.DAL.Entities;
using EcoStore.DAL.Repositories.Exceptions;
using EcoStore.DAL.Repositories.Interfaces;

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
        var payment = await _context.Payments.FindAsync(id)
            ?? throw new EntityNotFoundException($"Платіж з Id {id} не знайдено");
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
}