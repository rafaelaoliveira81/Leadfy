using Domain.Entities;
using Domain.Enuns;

namespace Application;

public class UserApp : IUserApp
{
    // Injeção de dependência do repositório de usuários e do serviço de hash de senhas
    private readonly IUserRepo _userRepo;
    private readonly IPasswordHasher _passwordHasher;
    
    // Construtor para injetar as dependências necessárias
    public UserApp(IUserRepo userRepo, IPasswordHasher passwordHasher)
    {
        _userRepo = userRepo;
        _passwordHasher = passwordHasher;
    }

    // Implementação do método para adicionar um novo usuário
    public async Task<int> AddAsync(User user, string password)
    {
        // Valida as informações do usuário (nome, email)
        ValidateUserInformation(user);

        // Valida a senha do usuário e gera o hash da senha utilizando o serviço de hash de senhas
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("A senha do usuário deve ser informada.");
        user.SetPassword(password, _passwordHasher);

        // Verifica se já existe um usuário com o mesmo e-mail para evitar duplicidade
        var userEntity = await _userRepo.GetByEmailAsync(user.Email);
        if (userEntity != null)
            throw new ArgumentException("Já existe usuário com o e-mail informado.");

        // Adiciona o usuário no banco de dados e retorna o ID do novo usuário criado
        return await _userRepo.AddAsync(user);
    }

    // Implementação do método para obter um usuário por ID
    public async Task<User> GetByIdAsync(int idUser)
    {
        // Verifica se existe usuário com o ID informado e retorna o usuário encontrado; 
        // caso contrário, lança uma exceção indicando que o usuário não foi localizado
        
        return await ValidateUserExistsByIdAsync(idUser);
    }

    // Implementação do método para obter um usuário por e-mail
    public async Task<User> GetByEmailAsync(string emailUser)
    {
        // Valida se o e-mail do usuário é nulo ou vazio
        if (string.IsNullOrWhiteSpace(emailUser))
            throw new ArgumentException("Email não pode ser vazio");

        // Verifica se existe um usuário com o e-mail informado e retorna o usuário encontrado;
        // caso contrário, lança uma exceção indicando que o usuário não foi localizado
        var userEntity = await _userRepo.GetByEmailAsync(emailUser);
        if (userEntity == null)
            throw new KeyNotFoundException("Usuário não localizado.");

        return userEntity;
    }

    // Implementação do método para obter um usuário por nome
    public async Task<User> GetByNameAsync(string nameUser)
    {
        // Valida se o nome do usuário é nulo ou vazio
        if (string.IsNullOrWhiteSpace(nameUser))
            throw new ArgumentException("Nome do usuário não pode ser vazio");

        // Verifica se existe um usuário com o nome informado e retorna o usuário encontrado;
        // caso contrário, lança uma exceção indicando que o usuário não foi localizado
        var userEntity = await _userRepo.GetByNameAsync(nameUser);
        if (userEntity == null)
            throw new KeyNotFoundException("Usuário não localizado.");

        return userEntity;
    }
    
    // Implementação do método para obter todos os usuários
    public async Task<IEnumerable<User>> GetAllAsync() => await _userRepo.GetAllAsync();
    
    // Implementação do método para obter todos os usuários por status (ativo/inativo)
    public async Task<IEnumerable<User>> GetAllByStatusAsync(bool statusUser) => await _userRepo.GetAllByStatusAsync(statusUser);

    // Implementação do método para atualizar as informações de um usuário existente
    public async Task UpdateAsync(User user)
    {
        // Garante que o usuário existe pelo ID; caso não exista, lança uma exceção
        var userEntity = await ValidateUserExistsByIdAsync(user.ID);

        // Valida os campos obrigatórios e regras de negócio do usuário
        ValidateUserInformation(user);

        // Verifica se já existe outro usuário utilizando o e-mail informado
        var userEntityByEmail = await _userRepo.GetByEmailAsync(user.Email);

        // Caso exista um usuário com o mesmo e-mail e 
        // que não seja o próprio usuário que está sendo atualizado, lança exceção
        if (userEntityByEmail != null && user.ID != userEntityByEmail.ID)
            throw new ArgumentException("Já existe um usuário com o e-mail informado.");

        // Atualiza os dados do usuário existente
        userEntity.Name = user.Name;
        userEntity.Email = user.Email;
        userEntity.Role = user.Role;

        // Salva as alterações no banco de dados
        await _userRepo.UpdateAsync(userEntity);
    }

    // Implementação do método para deletar um usuário
    public async Task DeleteAsync(int idUser)
    {
        // Garante que o usuário existe pelo ID; caso não exista, lança uma exceção
        var userEntity = await ValidateUserExistsByIdAsync(idUser);

        await _userRepo.DeleteAsync(userEntity);
    }

    // Implementação do método para desativar um usuário
    public async Task DeactivateAsync(int idUser)
    {
        var userEntity = await ValidateUserExistsByIdAsync(idUser);

        userEntity.Deactivate();

        await _userRepo.UpdateAsync(userEntity);
    }

    public async Task RestoreAsync(int idUser)
    {
        var userEntity = await ValidateUserExistsByIdAsync(idUser);

        userEntity.Activate();

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

    #region Uteis
    private static void ValidateUserInformation(User user)
    {
        // valida se o usuário é nulo
        if (user == null)
            throw new ArgumentException("Usuário não pode ser vazio.");
        
        // valida se o nome do usuário é nulo ou vazio
        if (string.IsNullOrWhiteSpace(user.Name))
            throw new ArgumentException("O nome do usuário deve ser informado.");

        // valida se o e-mail do usuário é nulo ou vazio
        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("O e-mail do usuário deve ser informado.");
        
        // valida se o tipo de usuário é válido (Admin, Manager, etc.)
        if (!Enum.IsDefined(typeof(UserRole), user.Role))
            throw new Exception("Tipo de usuário inválido.");
    }

    private async Task<User> ValidateUserExistsByIdAsync(int idUser)
    {
        // Verifica se existe um usuário com o ID informado; caso contrário, lança uma exceção indicando que o usuário não foi encontrado
        var userEntity = await _userRepo.GetByIdAsync(idUser);
        if (userEntity == null)
            throw new KeyNotFoundException("Usuário não localizado.");

        return userEntity;
    }
    #endregion
}