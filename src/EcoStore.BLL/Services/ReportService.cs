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
    private static readonly string[] s_productReportTableHeaderValues = new[] { "Id", "Назва", "Ціна", "Бренд", "Категорія", "Кількість на складі" };
    private static readonly string[] s_productSalesReportTableHeaderValues = new[] { "Id", "Назва", "Ціна", "Бренд", "Категорія", "Кількість продано", "Сума продажів" };
    private static readonly string[] s_brandsCategoriesSalesReportTableHeaderValues = new[] { "Id", "Назва", "Кількість продано", "Сума продажів" };
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

    public async Task<(byte[] Content, string FileName)> GetSalesReportAsync(
            SortSalesByDTO sortByDTO,
            bool descending,
            DateTime? startDate = null,
            DateTime? endDate = null)
    {
        var orders = (await _orderRepository.GetOrdersAsync(predicates: GetOrderPredicates(startDate, endDate))).ToList();
        var products = new Dictionary<int, Product>();
        var productsStatistics = new Dictionary<int, (int Quantity, decimal TotalPrice)>();
        foreach (var order in orders)
        {
            await AddOrderedProductsToStatistics(order, products, productsStatistics);
        }

        WriteSalesReport(products, productsStatistics, sortByDTO, descending, startDate, endDate);
        return (_htmlWriter.GetDocument(), GetFileName("sales"));
    }

    private void WriteSalesReport(
            Dictionary<int, Product> products,
            Dictionary<int, (int Quantity, decimal TotalPrice)> productsStatistics,
            SortSalesByDTO sortByDTO,
            bool descending,
            DateTime? startDate,
            DateTime? endDate)
    {
        _htmlWriter.Clear();
        _htmlWriter.AddStyles(CssStyles.BodyStyleFull + CssStyles.TableStyleFull);
        _htmlWriter.AddHeader("Звіт по продажах");
        if (startDate.HasValue || endDate.HasValue)
        {
            var paragraphText = "За період";
            if (startDate.HasValue)
            {
                paragraphText += $" з {startDate.Value:dd-MM-yyyy}";
            }

            if (endDate.HasValue)
            {
                paragraphText += $" до {endDate.Value:dd-MM-yyyy}";
            }

            _htmlWriter.AddParagraph(paragraphText);
        }

        WriteProductsSalesTable(products, productsStatistics, GetProductOrderedKeys(products, productsStatistics, sortByDTO, descending));
        var brands = products
            .GroupBy(p => p.Value.BrandId)
            .ToDictionary(g => g.Key, g => g.First().Value.Brand);
        var brandsStatistics = productsStatistics
            .GroupBy(p => products[p.Key].BrandId)
            .ToDictionary(g => g.Key, g => g.Select(p => p.Value).Aggregate((0, 0M), (sums, current) => (sums.Item1 + current.Quantity, sums.Item2 + current.TotalPrice)));
        WriteBrandsSalesTable(brands, brandsStatistics, GetBrandOrderedKeys(brands, brandsStatistics, sortByDTO, descending));
        var categories = products
            .GroupBy(p => p.Value.CategoryId)
            .ToDictionary(g => g.Key, g => g.First().Value.Category);
        var categoriesStatistics = productsStatistics
            .GroupBy(p => products[p.Key].CategoryId)
            .ToDictionary(g => g.Key, g => g.Select(p => p.Value).Aggregate((0, 0M), (sums, current) => (sums.Item1 + current.Quantity, sums.Item2 + current.TotalPrice)));
        WriteCategoriesSalesTable(categories, categoriesStatistics, GetCategoryOrderedKeys(categories, categoriesStatistics, sortByDTO, descending));
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

    private void WriteProductsSalesTable(
            Dictionary<int, Product> products,
            Dictionary<int, (int Quantity, decimal TotalPrice)> productsStatistics,
            IEnumerable<int> productIds)
    {
        _htmlWriter.AddSubHeader("По товарах");
        _htmlWriter.StartTable();
        _htmlWriter.AddTableHeader(s_productSalesReportTableHeaderValues);
        foreach (var id in productIds)
        {
            _htmlWriter.AddTableRow(new[]
            {
                id.ToString(CultureInfo.InvariantCulture),
                products[id].Name,
                products[id].Price.ToString(CultureInfo.InvariantCulture),
                products[id].Brand.Name,
                products[id].Category.Name,
                productsStatistics[id].Quantity.ToString(CultureInfo.InvariantCulture),
                productsStatistics[id].TotalPrice.ToString(CultureInfo.InvariantCulture),
            });
        }

        _htmlWriter.AddTableRow(new[]
        {
            "Всього",
            "",
            "",
            "",
            "",
            productsStatistics.Values.Sum(p => p.Quantity).ToString(CultureInfo.InvariantCulture),
            productsStatistics.Values.Sum(p => p.TotalPrice).ToString(CultureInfo.InvariantCulture),
        }, CssStyles.BoldTextStyleValue);
        _htmlWriter.EndTable();
    }

    private void WriteBrandsSalesTable(
            Dictionary<int, Brand> brands,
            Dictionary<int, (int Quantity, decimal TotalPrice)> brandStatistics,
            IEnumerable<int> brandIds)
    {
        _htmlWriter.AddSubHeader("По брендах");
        _htmlWriter.StartTable();
        _htmlWriter.AddTableHeader(s_brandsCategoriesSalesReportTableHeaderValues);
        foreach (var id in brandIds)
        {
            _htmlWriter.AddTableRow(new[]
            {
                id.ToString(CultureInfo.InvariantCulture),
                brands[id].Name,
                brandStatistics[id].Quantity.ToString(CultureInfo.InvariantCulture),
                brandStatistics[id].TotalPrice.ToString(CultureInfo.InvariantCulture),
            });
        }

        _htmlWriter.AddTableRow(new[]
        {
            "Всього",
            "",
            brandStatistics.Values.Sum(p => p.Quantity).ToString(CultureInfo.InvariantCulture),
            brandStatistics.Values.Sum(p => p.TotalPrice).ToString(CultureInfo.InvariantCulture),
        }, CssStyles.BoldTextStyleValue);
        _htmlWriter.EndTable();
    }

    private void WriteCategoriesSalesTable(
            Dictionary<int, Category> categories,
            Dictionary<int, (int Quantity, decimal TotalPrice)> categoriesStatistics,
            IEnumerable<int> categoryIds)
    {
        _htmlWriter.AddSubHeader("По категоріях");
        _htmlWriter.StartTable();
        _htmlWriter.AddTableHeader(s_brandsCategoriesSalesReportTableHeaderValues);
        foreach (var id in categoryIds)
        {
            _htmlWriter.AddTableRow(new[]
            {
                id.ToString(CultureInfo.InvariantCulture),
                categories[id].Name,
                categoriesStatistics[id].Quantity.ToString(CultureInfo.InvariantCulture),
                categoriesStatistics[id].TotalPrice.ToString(CultureInfo.InvariantCulture),
            });
        }

        _htmlWriter.AddTableRow(new[]
        {
            "Всього",
            "",
            categoriesStatistics.Values.Sum(p => p.Quantity).ToString(CultureInfo.InvariantCulture),
            categoriesStatistics.Values.Sum(p => p.TotalPrice).ToString(CultureInfo.InvariantCulture),
        }, CssStyles.BoldTextStyleValue);
        _htmlWriter.EndTable();
    }

    private void WriteProductsTable(
            List<Product> products,
            Dictionary<int, string> brandNames,
            Dictionary<int, string> categoryNames,
            int? highlightLowStockThreshold)
    {
        _htmlWriter.Clear();
        _htmlWriter.AddStyles(CssStyles.BodyStyleFull + CssStyles.TableStyleFull);
        _htmlWriter.AddHeader("Товари");
        _htmlWriter.StartTable();
        _htmlWriter.AddTableHeader(s_productReportTableHeaderValues);
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

    private async Task AddOrderedProductsToStatistics(
            Order order,
            Dictionary<int, Product> products,
            Dictionary<int, (int Quantity, decimal TotalPrice)> productsStatistics)
    {
        foreach (var orderedProduct in order.OrderedProducts)
        {
            if (!products.TryGetValue(orderedProduct.ProductId, out var product))
            {
                product = await _productRepository.GetProductByIdAsync(orderedProduct.ProductId);
                products[orderedProduct.ProductId] = product;
                productsStatistics[orderedProduct.ProductId] = (orderedProduct.Quantity, orderedProduct.TotalPrice);
            }
            else
            {
                var (quantity, totalPrice) = productsStatistics[product.Id];
                productsStatistics[product.Id] = (quantity + orderedProduct.Quantity, totalPrice + orderedProduct.TotalPrice);
            }
        }
    }

    private static Expression<Func<Product, object>> GetSortExpression(SortProductsInReportByDTO sortProductsInReportByDTO)
    {
        return sortProductsInReportByDTO switch
        {
            SortProductsInReportByDTO.Name => p => p.Name,
            SortProductsInReportByDTO.Stock => p => p.Stock,
            SortProductsInReportByDTO.Price => p => p.Price,
            SortProductsInReportByDTO.DateCreated => p => p.Id,
            _ => throw new ArgumentOutOfRangeException(nameof(sortProductsInReportByDTO), sortProductsInReportByDTO, null)
        };
    }

    private static IEnumerable<int> GetProductOrderedKeys(
            Dictionary<int, Product> products,
            Dictionary<int, (int Quantity, decimal TotalPrice)> productsStatistics,
            SortSalesByDTO sortByDTO, bool descending)
    {
        return sortByDTO switch
        {
            SortSalesByDTO.Name => descending
                ? products.OrderByDescending(p => p.Value.Name).Select(p => p.Key)
                : products.OrderBy(p => p.Value.Name).Select(p => p.Key),
            SortSalesByDTO.Revenue => descending
                ? productsStatistics.OrderByDescending(p => p.Value.TotalPrice).Select(p => p.Key)
                : productsStatistics.OrderBy(p => p.Value.TotalPrice).Select(p => p.Key),
            SortSalesByDTO.NumberSold => descending
                ? productsStatistics.OrderByDescending(p => p.Value.Quantity).Select(p => p.Key)
                : productsStatistics.OrderBy(p => p.Value.Quantity).Select(p => p.Key),
            SortSalesByDTO.DateCreated => descending
                ? products.OrderByDescending(p => p.Key).Select(p => p.Key)
                : products.OrderBy(p => p.Key).Select(p => p.Key),
            _ => throw new ArgumentOutOfRangeException(nameof(sortByDTO), sortByDTO, null),
        };
    }

    private static IEnumerable<int> GetCategoryOrderedKeys(
            Dictionary<int, Category> categories,
            Dictionary<int, (int Quantity, decimal TotalPrice)> categoriesStatistics,
            SortSalesByDTO sortByDTO, bool descending)
    {
        return sortByDTO switch
        {
            SortSalesByDTO.Name => descending
                ? categories.OrderByDescending(p => p.Value.Name).Select(p => p.Key)
                : categories.OrderBy(p => p.Value.Name).Select(p => p.Key),
            SortSalesByDTO.Revenue => descending
                ? categoriesStatistics.OrderByDescending(p => p.Value.TotalPrice).Select(p => p.Key)
                : categoriesStatistics.OrderBy(p => p.Value.TotalPrice).Select(p => p.Key),
            SortSalesByDTO.NumberSold => descending
                ? categoriesStatistics.OrderByDescending(p => p.Value.Quantity).Select(p => p.Key)
                : categoriesStatistics.OrderBy(p => p.Value.Quantity).Select(p => p.Key),
            SortSalesByDTO.DateCreated => descending
                ? categories.OrderByDescending(p => p.Key).Select(p => p.Key)
                : categories.OrderBy(p => p.Key).Select(p => p.Key),
            _ => throw new ArgumentOutOfRangeException(nameof(sortByDTO), sortByDTO, null),
        };
    }

    private static IEnumerable<int> GetBrandOrderedKeys(
            Dictionary<int, Brand> brands,
            Dictionary<int, (int Quantity, decimal TotalPrice)> brandsStatistics,
            SortSalesByDTO sortByDTO, bool descending)
    {
        return sortByDTO switch
        {
            SortSalesByDTO.Name => descending
                ? brands.OrderByDescending(p => p.Value.Name).Select(p => p.Key)
                : brands.OrderBy(p => p.Value.Name).Select(p => p.Key),
            SortSalesByDTO.Revenue => descending
                ? brandsStatistics.OrderByDescending(p => p.Value.TotalPrice).Select(p => p.Key)
                : brandsStatistics.OrderBy(p => p.Value.TotalPrice).Select(p => p.Key),
            SortSalesByDTO.NumberSold => descending
                ? brandsStatistics.OrderByDescending(p => p.Value.Quantity).Select(p => p.Key)
                : brandsStatistics.OrderBy(p => p.Value.Quantity).Select(p => p.Key),
            SortSalesByDTO.DateCreated => descending
                ? brands.OrderByDescending(p => p.Key).Select(p => p.Key)
                : brands.OrderBy(p => p.Key).Select(p => p.Key),
            _ => throw new ArgumentOutOfRangeException(nameof(sortByDTO), sortByDTO, null),
        };
    }

    private static bool AddRowStyle(Product product, int? highlightLowStockThreshold)
    {
        return highlightLowStockThreshold is not null && product.Stock <= highlightLowStockThreshold;
    }

    private static IEnumerable<Expression<Func<Order, bool>>> GetOrderPredicates(DateTime? startDate, DateTime? endDate)
    {
        var list = new List<Expression<Func<Order, bool>>>();
        if (startDate is not null)
        {
            list.Add(o => o.OrderDate >= startDate);
        }

        if (endDate is not null)
        {
            list.Add(o => o.OrderDate <= endDate);
        }

        return list;
    }
}