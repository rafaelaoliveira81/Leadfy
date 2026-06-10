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

        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Email e senha são obrigatórios.");

        var email = request.Email.Trim();
        User user;

        if (!string.IsNullOrWhiteSpace(request.TenantId))
        {
            if (!Guid.TryParse(request.TenantId, out var tenantGuid))
                throw new ArgumentException("TenantId inválido.");

            user = (await _userRepo.GetByEmailAnyTenantAsync(email))
                .FirstOrDefault(currentUser => currentUser.TenantId == tenantGuid);
        }
        else
        {
            var usersByEmail = await _userRepo.GetByEmailAnyTenantAsync(email);

            if (usersByEmail.Count > 1)
                throw new ArgumentException("Há mais de um usuário com este e-mail em tenants diferentes. Informe o TenantId no login.");

            user = usersByEmail.FirstOrDefault();
        }

        if (user == null || !user.IsActive || !VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Email ou senha inválidos.");

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