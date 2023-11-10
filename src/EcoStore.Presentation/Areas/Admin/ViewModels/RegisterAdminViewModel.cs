namespace EcoStore.Presentation.Areas.Admin.ViewModels;

public class RegisterAdminViewModel
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string ConfirmPassword { get; set; } = default!;
}