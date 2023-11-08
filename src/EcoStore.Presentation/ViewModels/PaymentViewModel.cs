namespace EcoStore.Presentation.ViewModels;

public class PaymentViewModel
{
    public string Id { get; set; } = default!;

    public DateTime Created { get; set; }

    public int Amount { get; set; }

    public string Currency { get; set; } = default!;
}