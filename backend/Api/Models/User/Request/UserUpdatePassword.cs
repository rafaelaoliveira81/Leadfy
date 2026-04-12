namespace Models.Request;

public class UserUpdatePassword
{
    public string NewPassword { get; set; }
    public string CurrentPassword { get; set; }
}