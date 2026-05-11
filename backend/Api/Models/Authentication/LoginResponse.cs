namespace Models.Response;

public class LoginResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }
    public string Name { get; set; }
    public List<string> Permissions { get; set; }

    public LoginResponse()
    {
        Permissions = new List<string>();
    }
}
