namespace Application.DTO;

public class UserUpdatePasswordRequest
{
    public string NewPassword { get; set; }
    public string CurrentPassword { get; set; }
}
