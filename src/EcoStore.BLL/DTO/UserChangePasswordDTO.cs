namespace EcoStore.BLL.DTO;

public class UserChangePasswordDTO
{
    public string Email { get; set; } = default!;

    public string OldPassword { get; set; } = default!;

    public string NewPassword { get; set; } = default!;

    public string ConfirmNewPassword { get; set; } = default!;
}