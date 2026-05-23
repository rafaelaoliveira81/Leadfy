using Application.DTO;

namespace Application;

public class AuthenticationApp : IAuthenticationApp
{
    private readonly IUserRepo _userRepo;
    private readonly IPasswordHasher _passwordHasher;

    public AuthenticationApp(
        IUserRepo userRepo,
        IPasswordHasher passwordHasher)
    {
        _userRepo = userRepo;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        if (request == null)
            throw new ArgumentException("Requisição inválida.");

        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Email e senha são obrigatórios.");

        var user = await _userRepo.GetByEmailWithGroupAsync(request.Email.Trim());
        if (user == null || !_passwordHasher.Verify(user.PasswordHash, request.Password))
            throw new UnauthorizedAccessException("Email ou senha inválidos.");

        return new LoginResponse
        {
            Success = true,
            Name = user.Name,
            Message = "Login realizado com sucesso."
        };
    }
}