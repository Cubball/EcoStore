namespace EcoStore.Presentation.ViewModels;

public class AppUserViewModel
{
    public string Id { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public RoleViewModel Role { get; set; }
}