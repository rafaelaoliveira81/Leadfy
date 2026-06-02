using System.Security.Cryptography;
using Application.DTO;
using Domain.Entities;

namespace Application;

public class UserApp : IUserApp
{
    private readonly IUserRepo _userRepo;
    public UserApp(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<int> AddAsync(UserRequest request)
    {
        ValidateUserInformation(request);

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("A senha do usuário deve ser informada.");

        var userEntity = await _userRepo.GetByEmailAsync(request.Email);
        if (userEntity != null)
            throw new ArgumentException("Já existe usuário com o e-mail informado.");

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = PasswordHasher(request.Password)
        };

        return await _userRepo.AddAsync(user);
    }

    public async Task<UserResponse> GetByIdAsync(int idUser)
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
        var users = await _userRepo.GetPagedAsync(isActive, pagina, quantidadePorPagina);

        var response = users.Dados.Select(MapToUserResponse).ToList();

        return new UserPagedResponse
        {
            TotalRegistros = users.TotalRegistros,
            Dados = response
        };
    }

    public async Task UpdateAsync(int id, UserRequest request)
    {
        var user = await ValidateUserExistsByIdAsync(id);

        ValidateUserInformation(request);

        var userByEmail = await _userRepo.GetByEmailAsync(request.Email);

        if (userByEmail != null && id != userByEmail.ID)
            throw new ArgumentException("Já existe um usuário com o e-mail informado.");

        user.Name = request.Name;
        user.Email = request.Email;

        if (!string.IsNullOrWhiteSpace(request.Password))
            user.PasswordHash = PasswordHasher(request.Password);

        await _userRepo.UpdateAsync(user);
    }

    public async Task UpdatePasswordAsync(int id, UserUpdatePasswordRequest request)
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
    public async Task DeleteAsync(int idUser)
    {
        var userEntity = await ValidateUserExistsByIdAsync(idUser);

        await _userRepo.DeleteAsync(userEntity);
    }
    public async Task DeactivateAsync(int idUser)
    {
        var userEntity = await ValidateUserExistsByIdAsync(idUser);

        userEntity.Deactivate();

        await _userRepo.UpdateAsync(userEntity);
    }
    public async Task ActivateAsync(int idUser)
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

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("O e-mail do usuário deve ser informado.");
    }
    private async Task<User> ValidateUserExistsByIdAsync(int idUser)
    {
        var userEntity = await _userRepo.GetByIdAsync(idUser);
        if (userEntity == null)
            throw new KeyNotFoundException("Usuário não localizado.");

        return userEntity;
    }
    private static UserResponse MapToUserResponse(User user)
    {
        return new UserResponse
        {
            ID = user.ID,
            Name = user.Name,
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