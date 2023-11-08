using EcoStore.BLL.DTO;
using EcoStore.Presentation.ViewModels;

namespace EcoStore.Presentation.Mapping;

public static class PaymentMappingExtensions
{
    public static PaymentViewModel ToViewModel(this PaymentDTO paymentDTO)
    {
        return new PaymentViewModel
        {
            Id = paymentDTO.Id,
            Created = paymentDTO.Created,
            Amount = paymentDTO.Amount,
            Currency = paymentDTO.Currency,
        };
    }
}