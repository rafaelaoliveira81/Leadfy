using Application.DTO;
namespace Application;

public class AuthenticationApp : IAuthenticationApp
{
    private readonly IUserRepo _userRepo;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthenticationApp(
        IUserRepo userRepo,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepo = userRepo;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
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

        var token = _tokenService.GenerateToken(user);

        return new LoginResponse
        {
            Token = token,
            Name = user.Name,
            UserId = user.ID,
            Email = user.Email
        };
    }
}