namespace EcoStore.Presentation.ViewModels;

public class LoginViewModel
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public bool RememberMe { get; set; }
}