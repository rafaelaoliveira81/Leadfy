using Application.DTO;
public interface IAuthenticationApp
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}