namespace Models.Response;

public class UserResponse
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public UserRoleResponse Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}