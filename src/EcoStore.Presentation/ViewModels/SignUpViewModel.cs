using System.ComponentModel.DataAnnotations;

namespace EcoStore.Presentation.ViewModels;

public class SignUpViewModel
{
    [EmailAddress]
    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    // TODO: do i need attributes for validation?
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = default!;

    [Phone]
    public string PhoneNumber { get; set; } = default!;
}