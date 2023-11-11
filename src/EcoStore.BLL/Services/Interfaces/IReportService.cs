using EcoStore.BLL.DTO;

namespace EcoStore.BLL.Services.Interfaces;

public interface IReportService
{
    Task<(byte[] Content, string FileName)> GetProductsReportAsync(
            SortProductsInReportByDTO sortByDTO,
            bool descending,
            int? highlightLowStockThreshold = null);

    Task<string> GetOrdersReportAsync(DateTime? startDate = null, DateTime? endDate = null);
}