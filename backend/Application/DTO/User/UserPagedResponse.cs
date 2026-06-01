namespace Application.DTO;

public class UserPagedResponse
{
    public int TotalRegistros { get; set; }
    public List<UserResponse> Dados { get; set; } = new();
}
