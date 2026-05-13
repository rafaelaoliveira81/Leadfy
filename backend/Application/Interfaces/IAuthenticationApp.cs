using Application.DTO;

namespace Application;

public interface IAuthenticationApp
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}