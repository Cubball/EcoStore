namespace EcoStore.BLL.DTO;

public class AdminRegisterDTO
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string ConfirmPassword { get; set; } = default!;
}