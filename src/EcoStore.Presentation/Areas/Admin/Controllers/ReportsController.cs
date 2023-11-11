using EcoStore.BLL.Services.Interfaces;
using EcoStore.Presentation.Areas.Admin.Mapping;
using EcoStore.Presentation.Areas.Admin.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoStore.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ReportsController : Controller
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Sales()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Sales(SalesReportOptionsViewModel salesReportOptions)
    {
        DateTime? startDate = salesReportOptions.From.HasValue
            ? salesReportOptions.From.Value.ToDateTime(new TimeOnly(0, 0, 0), DateTimeKind.Local).ToUniversalTime()
            : null;
        DateTime? endDate = salesReportOptions.To.HasValue
            ? salesReportOptions.To.Value.ToDateTime(new TimeOnly(0, 0, 0), DateTimeKind.Local).ToUniversalTime()
            : null;
        var (content, fileName) = await _reportService.GetSalesReportAsync(
                salesReportOptions.SortBy.ToDTO(),
                salesReportOptions.Descending,
                startDate,
                endDate);
        return File(content, "text/html", fileName);
    }

    public IActionResult Products()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Products(ProductsReportOptionsViewModel productsReportOptions)
    {
        var (content, fileName) = await _reportService.GetProductsReportAsync(
                productsReportOptions.SortBy.ToDTO(),
                productsReportOptions.Descending,
                productsReportOptions.HighlightLowStockProducts ? productsReportOptions.LowStockThreshold : null);
        return File(content, "text/html", fileName);
    }

    public IActionResult Orders()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Orders(OrdersReportOptionsViewModel ordersReportOptions)
    {
        DateTime? startDate = ordersReportOptions.From.HasValue
            ? ordersReportOptions.From.Value.ToDateTime(new TimeOnly(0, 0, 0), DateTimeKind.Local).ToUniversalTime()
            : null;
        DateTime? endDate = ordersReportOptions.To.HasValue
            ? ordersReportOptions.To.Value.ToDateTime(new TimeOnly(23, 59, 59), DateTimeKind.Local).ToUniversalTime()
            : null;
        var (content, fileName) = await _reportService.GetOrdersReportAsync(startDate, endDate);
        return File(content, "text/html", fileName);
    }
}