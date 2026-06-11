using System.Security.Cryptography;
using Application.DTO;
using Domain.Entities;
using Domain.Interface;

namespace Application;

public class UserApp : IUserApp
{
    private readonly IUserRepo _userRepo;
    private readonly ITenantRepo _tenantRepo;
    private readonly ITenantProvider _tenantProvider;

    public UserApp(IUserRepo userRepo, ITenantRepo tenantRepo, ITenantProvider tenantProvider)
    {
        _userRepo = userRepo;
        _tenantRepo = tenantRepo;
        _tenantProvider = tenantProvider;
    }

    public async Task<string> AddAsync(UserRequest request)
    {
        ValidateUserInformation(request);
        var tenantGuid = _tenantProvider.GetRequiredTenantId();

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("A senha do usuário deve ser informada.");

        var userEntity = await _userRepo.GetByEmailAsync(request.Email);
        if (userEntity != null)
            throw new ArgumentException("Já existe usuário com o e-mail informado.");

        var userByUserName = await _userRepo.GetByUserNameGlobalAsync(request.UserName);
        if (userByUserName != null)
            throw new ArgumentException("Este nome já está sendo utilizado, teste outro.");

        var user = new User
        {
            TenantId = tenantGuid,
            Name = request.Name,
            UserName = request.UserName,
            Email = request.Email,
            PasswordHash = PasswordHasher(request.Password)
        };

        return (await _userRepo.CreateAsync(user)).ToString();
    }
    public async Task<string> RegisterAsync(UserRequest request)
    {
        ValidateUserInformation(request);

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("A senha do usuário deve ser informada.");

        var userByUserName = await _userRepo.GetByUserNameGlobalAsync(request.UserName);
        if (userByUserName != null)
            throw new ArgumentException("Este nome já está sendo utilizado, teste outro.");

        var tenant = new Tenant
        {
            Name = BuildDefaultTenantName(request.Name)
        };

        await _tenantRepo.CreateAsync(tenant);

        var user = new User
        {
            TenantId = tenant.Id,
            Name = request.Name,
            UserName = request.UserName,
            Email = request.Email,
            PasswordHash = PasswordHasher(request.Password)
        };

        return (await _userRepo.CreateAsync(user)).ToString();
    }

    public async Task<UserResponse> GetByIdAsync(string idUser)
    {
        var user = await ValidateUserExistsByIdAsync(idUser);

        return MapToUserResponse(user);
    }

    public async Task<UserResponse> GetByEmailAsync(string emailUser)
    {
        if (string.IsNullOrWhiteSpace(emailUser))
            throw new ArgumentException("Email não pode ser vazio");

        var user = await _userRepo.GetByEmailAsync(emailUser);
        if (user == null)
            throw new KeyNotFoundException("Usuário não localizado.");

        return MapToUserResponse(user);
    }

    public async Task<UserPagedResponse> GetAllAsync(bool? isActive, int pagina, int quantidadePorPagina)
    {
        var tenantId = _tenantProvider.GetRequiredTenantId();

        var users = await _userRepo.GetPagedAsync(tenantId, isActive, pagina, quantidadePorPagina);

        var response = users.Dados.Select(MapToUserResponse).ToList();

        return new UserPagedResponse
        {
            TotalRegistros = users.TotalRegistros,
            Dados = response
        };
    }

    public async Task UpdateAsync(string id, UserRequest request)
    {
        var user = await ValidateUserExistsByIdAsync(id);

        ValidateUserInformation(request);

        var userByEmail = await _userRepo.GetByEmailAsync(request.Email);

        if (userByEmail != null && id != userByEmail.Id.ToString())
            throw new ArgumentException("Já existe um usuário com o e-mail informado.");

        var userByUserName = await _userRepo.GetByUserNameGlobalAsync(request.UserName);

        if (userByUserName != null && id != userByUserName.Id.ToString())
            throw new ArgumentException("Este nome já está sendo utilizado, teste outro.");

        user.Name = request.Name;
        user.UserName = request.UserName;
        user.Email = request.Email;

        if (!string.IsNullOrWhiteSpace(request.Password))
            user.PasswordHash = PasswordHasher(request.Password);

        await _userRepo.UpdateAsync(user);
    }

    public async Task UpdatePasswordAsync(string id, UserUpdatePasswordRequest request)
    {
        var user = await ValidateUserExistsByIdAsync(id);

        if (string.IsNullOrWhiteSpace(request.CurrentPassword))
            throw new ArgumentException("A senha atual deve ser informada.");

        if (string.IsNullOrWhiteSpace(request.NewPassword))
            throw new ArgumentException("A nova senha deve ser informada.");

        if (!VerifyPassword(request.CurrentPassword, user.PasswordHash))
            throw new UnauthorizedAccessException("Senha atual inválida.");

        if (request.CurrentPassword == request.NewPassword)
            throw new ArgumentException("A nova senha deve ser diferente da senha atual.");

        user.PasswordHash = PasswordHasher(request.NewPassword);

        await _userRepo.UpdateAsync(user);
    }
    public async Task DeleteAsync(string idUser)
    {
        var userEntity = await ValidateUserExistsByIdAsync(idUser);

        await _userRepo.DeleteAsync(userEntity);
    }
    public async Task DeactivateAsync(string idUser)
    {
        var userEntity = await ValidateUserExistsByIdAsync(idUser);

        userEntity.Deactivate();

        await _userRepo.UpdateAsync(userEntity);
    }
    public async Task ActivateAsync(string idUser)
    {
        var userEntity = await ValidateUserExistsByIdAsync(idUser);

        userEntity.Activate();

        await _userRepo.UpdateAsync(userEntity);
    }

    #region Utils
    private static void ValidateUserInformation(UserRequest request)
    {
        if (request == null)
            throw new ArgumentException("Usuário não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("O nome do usuário deve ser informado.");

        if (string.IsNullOrWhiteSpace(request.UserName))
            throw new ArgumentException("O UserName do usuário deve ser informado.");

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("O e-mail do usuário deve ser informado.");
    }
    private async Task<User> ValidateUserExistsByIdAsync(string idUser)
    {
        if (!Guid.TryParse(idUser, out var guid))
            throw new ArgumentException("ID do usuário inválido.");

        var userEntity = await _userRepo.GetByIdAsync(guid);
        
        if (userEntity == null)
            throw new KeyNotFoundException("Usuário não localizado.");

        return userEntity;
    }

    private static string BuildDefaultTenantName(string userName)
    {
        var trimmed = userName?.Trim();

        if (string.IsNullOrWhiteSpace(trimmed))
            return "Novo Tenant";

        return $"Tenant {trimmed}";
    }

    private static UserResponse MapToUserResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id.ToString(),
            Name = user.Name,
            UserName = user.UserName,
            Email = user.Email,
            IsActive = user.IsActive
        };
    }

    private string PasswordHasher(string password)
    {
        const int iterations = 10000;
        const int saltSize = 16;
        const int keySize = 32;

        using var algorithm = new Rfc2898DeriveBytes(
            password,
            saltSize,
            iterations,
            HashAlgorithmName.SHA256
        );

        string salt = Convert.ToBase64String(algorithm.Salt);
        string key = Convert.ToBase64String(algorithm.GetBytes(keySize));

        return $"{iterations}.{salt}.{key}";
    }

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