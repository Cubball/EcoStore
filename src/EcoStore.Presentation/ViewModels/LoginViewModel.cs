using System.ComponentModel.DataAnnotations;

namespace EcoStore.Presentation.ViewModels;

public class LoginViewModel
{
    [EmailAddress]
    public string Email { get; set; } = default!;

    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    public bool RememberMe { get; set; }
}