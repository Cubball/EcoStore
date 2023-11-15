namespace EcoStore.Presentation.Areas.Admin.ViewModels;

public class SalesReportOptionsViewModel
{
    public SortSalesByViewModel SortBy { get; set; }

    public DateOnly? From { get; set; }

    public DateOnly? To { get; set; }
}