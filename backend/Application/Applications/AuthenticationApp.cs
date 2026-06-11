using System.Security.Cryptography;
using Application.DTO;
using Domain.Entities;
namespace Application;

public class AuthenticationApp : IAuthenticationApp
{
    private readonly IUserRepo _userRepo;
    private readonly ITokenService _tokenService;

    public AuthenticationApp(
        IUserRepo userRepo,
        ITokenService tokenService)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        if (request == null)
            throw new ArgumentException("Requisição inválida.");

        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Nome de usuário e senha são obrigatórios.");

        var userName = request.UserName.Trim();

        var user = await _userRepo.GetByUserNameGlobalAsync(userName);

        if (user == null || !user.IsActive || !VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Usuário ou senha inválidos.");

        var token = _tokenService.GenerateToken(user);

        return new LoginResponse
        {
            Token = token,
            Name = user.Name,
            Message = "Login efetuado com sucesso."
        };
    }

    #region Utils

    private bool VerifyPassword(string password, string storedHash)
    {
        var parts = storedHash.Split('.');

        int iterations = int.Parse(parts[0]);
        byte[] salt = Convert.FromBase64String(parts[1]);
        string savedKey = parts[2];

        using var algorithm = new Rfc2898DeriveBytes(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256
        );

        string generatedKey = Convert.ToBase64String(algorithm.GetBytes(32));

        return generatedKey == savedKey;
    }

    #endregion
}