namespace EcoStore.BLL.DTO;

public class ProductsFilterDTO
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int[] CategoryIds { get; set; } = default!;

    public int[] BrandIds { get; set; } = default!;

    public int? MinPrice { get; set; }

    public int? MaxPrice { get; set; }

    public string? SearchString { get; set; }

    public SortBy SortBy { get; set; }

    public bool Descending { get; set; }
}