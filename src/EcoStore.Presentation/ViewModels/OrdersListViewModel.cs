namespace EcoStore.Presentation.ViewModels;

public class OrdersListViewModel
{
    public PageInfoViewModel PageInfo { get; set; } = default!;

    public IEnumerable<OrderViewModel> Orders { get; set; } = default!;
}