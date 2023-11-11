using EcoStore.BLL.DTO;

namespace EcoStore.BLL.Services.Interfaces;

public interface IReportService
{
    Task<(byte[] Content, string FileName)> GetProductsReportAsync(
            SortProductsInReportByDTO sortByDTO,
            bool descending,
            int? highlightLowStockThreshold = null);

    Task<(byte[] Content, string FileName)> GetOrdersReportAsync(
            DateTime? startDate = null,
            DateTime? endDate = null);

    Task<(byte[] Content, string FileName)> GetSalesReportAsync(
            SortSalesByDTO sortByDTO,
            bool descending,
            DateTime? startDate = null,
            DateTime? endDate = null);
}