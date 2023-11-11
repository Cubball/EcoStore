using System.Globalization;
using System.Linq.Expressions;

using EcoStore.BLL.DTO;
using EcoStore.BLL.Infrastructure;
using EcoStore.BLL.Services.Interfaces;
using EcoStore.DAL.Entities;
using EcoStore.DAL.Repositories.Interfaces;

namespace EcoStore.BLL.Services;

public class ReportService : IReportService
{
    private readonly string[] _productReportTableHeaderValues = new[] { "Id", "Назва", "Ціна", "Бренд", "Категорія", "Кількість на складі" };
    private readonly IClock _clock;
    private readonly IHtmlWriter _htmlWriter;
    private readonly IProductRepository _productRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IOrderRepository _orderRepository;

    public ReportService(
            IClock clock,
            IHtmlWriter htmlWriter,
            IProductRepository productRepository,
            IBrandRepository brandRepository,
            ICategoryRepository categoryRepository,
            IOrderRepository orderRepository)
    {
        _clock = clock;
        _htmlWriter = htmlWriter;
        _productRepository = productRepository;
        _brandRepository = brandRepository;
        _categoryRepository = categoryRepository;
        _orderRepository = orderRepository;
    }

    public Task<string> GetOrdersReportAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        throw new NotImplementedException();
    }

    public async Task<(byte[] Content, string FileName)> GetProductsReportAsync(
            SortProductsInReportByDTO sortByDTO,
            bool descending,
            int? highlightLowStockThreshold = null)
    {
        var products = (await _productRepository.GetProductsAsync(
                orderBy: GetSortExpression(sortByDTO),
                descending: descending))
            .ToList();
        var brandNames = new Dictionary<int, string>();
        var categoryNames = new Dictionary<int, string>();
        await FillBrandsAndCategoriesNamesForProducts(products, brandNames, categoryNames);
        WriteProductsTable(products, brandNames, categoryNames, highlightLowStockThreshold);
        return (_htmlWriter.GetDocument(), GetFileName("products"));
    }

    private void WriteProductsTable(
            List<Product> products,
            Dictionary<int, string> brandNames,
            Dictionary<int, string> categoryNames,
            int? highlightLowStockThreshold)
    {
        _htmlWriter.AddStyles(CssStyles.BodyStyleFull + CssStyles.TableStyleFull);
        _htmlWriter.AddHeader("Товари");
        _htmlWriter.StartTable();
        _htmlWriter.AddTableHeader(_productReportTableHeaderValues);
        foreach (var product in products)
        {
            _htmlWriter.AddTableRow(new[]
            {
                product.Id.ToString(CultureInfo.InvariantCulture),
                product.Name,
                product.Price.ToString("N2", CultureInfo.InvariantCulture),
                brandNames[product.BrandId],
                categoryNames[product.CategoryId],
                product.Stock.ToString(CultureInfo.InvariantCulture)
            },
            AddRowStyle(product, highlightLowStockThreshold) ? CssStyles.LowStockRowStyleValue : null);
        }

        _htmlWriter.EndTable();
    }

    private async Task FillBrandsAndCategoriesNamesForProducts(
            List<Product> products,
            Dictionary<int, string> brandNames,
            Dictionary<int, string> categoryNames)
    {
        foreach (var product in products)
        {
            if (!brandNames.ContainsKey(product.BrandId))
            {
                var brand = await _brandRepository.GetBrandByIdAsync(product.BrandId);
                brandNames[product.BrandId] = brand.Name;
            }

            if (!categoryNames.ContainsKey(product.CategoryId))
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);
                categoryNames[product.CategoryId] = category.Name;
            }
        }
    }

    private string GetFileName(string prefix)
    {
        return $"{prefix}_report_{_clock.UtcNow.ToLocalTime():HH-mm-ss_dd-MM-yyyy}.html";
    }

    private static Expression<Func<Product, object>> GetSortExpression(SortProductsInReportByDTO sortProductsInReportByDTO)
    {
        return sortProductsInReportByDTO switch
        {
            SortProductsInReportByDTO.Name => p => p.Name,
            SortProductsInReportByDTO.Stock => p => p.Stock,
            SortProductsInReportByDTO.DateCreated => p => p.Id,
            _ => throw new ArgumentOutOfRangeException(nameof(sortProductsInReportByDTO), sortProductsInReportByDTO, null)
        };
    }

    private static bool AddRowStyle(Product product, int? highlightLowStockThreshold)
    {
        return highlightLowStockThreshold is not null && product.Stock <= highlightLowStockThreshold;
    }
}