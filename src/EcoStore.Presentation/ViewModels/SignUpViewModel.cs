namespace EcoStore.Presentation.ViewModels;

public class SignUpViewModel
{
    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string ConfirmPassword { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;
}