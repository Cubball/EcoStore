namespace EcoStore.Presentation.Areas.Admin.ViewModels;

public class UpdateCategoryViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;
}