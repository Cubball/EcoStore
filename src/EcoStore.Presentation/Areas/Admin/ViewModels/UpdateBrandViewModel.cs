namespace EcoStore.Presentation.Areas.Admin.ViewModels;

public class UpdateBrandViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;
}