using Application.DTO;

namespace Application;

public class AuthenticationApp : IAuthenticationApp
{
    private readonly IUserRepo _userRepo;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserGroupPermissionApp _userGroupPermissionApp;
    private readonly IJwtApp _jwtApp;

    public AuthenticationApp(
        IUserRepo userRepo,
        IPasswordHasher passwordHasher,
        IUserGroupPermissionApp userGroupPermissionApp,
        IJwtApp jwtApp)
    {
        _userRepo = userRepo;
        _passwordHasher = passwordHasher;
        _userGroupPermissionApp = userGroupPermissionApp;
        _jwtApp = jwtApp;
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

        var permissions = await _userGroupPermissionApp.GetPermissionNamesAsync(user);
        var token = _jwtApp.GenerateToken(user, permissions);

        return new LoginResponse
        {
            Success = true,
            Name = user.Name,
            Message = "Login realizado com sucesso.",
            Token = token,
            Permissions = permissions.ToList()
        };
    }
}