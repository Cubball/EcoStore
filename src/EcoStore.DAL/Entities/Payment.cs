namespace EcoStore.DAL.Entities;

public class Payment
{
    public int Id { get; set; }

    public string TransactionId { get; set; } = default!;

    public string Status { get; set; } = default!;
}