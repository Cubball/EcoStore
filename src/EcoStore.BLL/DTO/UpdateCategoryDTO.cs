namespace EcoStore.BLL.DTO;

public class UpdateCategoryDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;
}