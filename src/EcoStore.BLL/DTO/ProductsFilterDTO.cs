namespace EcoStore.BLL.DTO;

// TODO: sorting by and asc or desc
public class ProductsFilterDTO
{
    public int? PageNumber { get; set; }

    public int? PageSize { get; set; }

    public int[]? CategoryIds { get; set; }

    public int[]? BrandIds { get; set; }

    public int? MinPrice { get; set; }

    public int? MaxPrice { get; set; }

    public string? SearchString { get; set; }
}