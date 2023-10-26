using EcoStore.BLL.DTO;
using EcoStore.DAL.Entities;

namespace EcoStore.BLL.Mapping;

public static class PaymentMappingExtensions
{
    public static PaymentDTO ToDTO(this Payment payment)
    {
        return new PaymentDTO
        {
            Id = payment.Id,
            Created = payment.Created,
            Amount = payment.Amount,
            Currency = payment.Currency,
        };
    }
}