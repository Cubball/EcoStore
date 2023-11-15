namespace EcoStore.Presentation.Areas.Admin.ViewModels;

public class ProductsReportOptionsViewModel
{
    public SortProductsInReportByViewModel SortBy { get; set; }

    public bool HighlightLowStockProducts { get; set; }

    public int LowStockThreshold { get; set; }
}