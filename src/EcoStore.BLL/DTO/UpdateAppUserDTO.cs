namespace EcoStore.BLL.DTO;

public class UpdateAppUserDTO
{
    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;
}