using Domain.Entities;
using Domain.Enuns;

namespace Application;

public class UserApp : IUserApp
{
    private readonly IUserRepo _userRepo;
    private readonly IPasswordHasher _passwordHasher;
    public UserApp(IUserRepo userRepo, IPasswordHasher passwordHasher)
    {
        _userRepo = userRepo;
        _passwordHasher = passwordHasher;
    }
    public async Task<int> AddAsync(User user, string password)
    {
        ValidateUserInformation(user);

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("A senha do usuário deve ser informada.");

        var userEntity = await _userRepo.GetByEmailAsync(user.Email);
        if (userEntity != null)
            throw new ArgumentException("Já existe usuário com o e-mail informado.");

        user.SetPassword(password, _passwordHasher);

        return await _userRepo.AddAsync(user);
    }
    public async Task<User> GetByIdAsync(int idUser)
    {
        return await ValidateUserExistsByIdAsync(idUser);
    }
    public async Task<User> GetByEmailAsync(string emailUser)
    {
        if (string.IsNullOrWhiteSpace(emailUser))
            throw new ArgumentException("Email não pode ser vazio");

        var userEntity = await _userRepo.GetByEmailAsync(emailUser);
        if (userEntity == null)
            throw new KeyNotFoundException("Usuário não localizado.");

        return userEntity;
    }
    public async Task<IEnumerable<User>> GetByNameContainingAsync(string nameUser)
    {
        if (string.IsNullOrWhiteSpace(nameUser))
            throw new ArgumentException("Nome do usuário não pode ser vazio");

        nameUser = nameUser.Trim();

        var userEntity = await _userRepo.GetByNameContainingAsync(nameUser);
        if (userEntity == null)
            throw new KeyNotFoundException("Usuário não localizado.");

        return userEntity;
    }
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userRepo.GetAllAsync();
    }
    public async Task<IEnumerable<User>> GetAllByStatusAsync(bool statusUser)
    {
        return await _userRepo.GetAllByStatusAsync(statusUser);
    }
    public async Task UpdateAsync(User user)
    {
        var userEntity = await ValidateUserExistsByIdAsync(user.ID);

        ValidateUserInformation(user);

        var userEntityByEmail = await _userRepo.GetByEmailAsync(user.Email);

        if (userEntityByEmail != null && user.ID != userEntityByEmail.ID)
            throw new ArgumentException("Já existe um usuário com o e-mail informado.");

        userEntity.Name = user.Name;
        userEntity.Email = user.Email;

        await _userRepo.UpdateAsync(userEntity);
    }
    public async Task UpdatePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        var userEntity = await ValidateUserExistsByIdAsync(userId);

        if (string.IsNullOrWhiteSpace(currentPassword))
            throw new ArgumentException("A senha atual deve ser informada.");

        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("A nova senha deve ser informada.");

        var isCurrentPasswordValid = _passwordHasher.Verify(userEntity.PasswordHash, currentPassword);

        if (!isCurrentPasswordValid)
            throw new UnauthorizedAccessException("Senha atual incorreta.");

        userEntity.SetPassword(newPassword, _passwordHasher);

        await _userRepo.UpdateAsync(userEntity);
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


    #region Métodos auxiliares
    private static void ValidateUserInformation(User user)
    {
        if (user == null)
            throw new ArgumentException("Usuário não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(user.Name))
            throw new ArgumentException("O nome do usuário deve ser informado.");

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("O e-mail do usuário deve ser informado.");
    }
    private async Task<User> ValidateUserExistsByIdAsync(int idUser)
    {
        var userEntity = await _userRepo.GetByIdAsync(idUser);
        if (userEntity == null)
            throw new KeyNotFoundException("Usuário não localizado.");

        return userEntity;
    }

    #endregion
}