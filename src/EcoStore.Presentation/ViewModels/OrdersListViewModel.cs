namespace EcoStore.Presentation.ViewModels;

public class OrdersListViewModel
{
    public PageInfoViewModel PageInfo { get; set; } = default!;

    public IEnumerable<OrderViewModel> Orders { get; set; } = default!;

    public string? From { get; set; }

    public string? To { get; set; }
}