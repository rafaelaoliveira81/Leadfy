using Domain.Entities;
using Domain.Enuns;

namespace Application;

/// <summary>
/// Serviço de aplicação responsável por orquestrar os casos de uso relacionados a usuários.
/// </summary>
/// <remarks>
/// Esta classe pertence à camada de Application.
/// Sua responsabilidade é validar entradas, aplicar regras de fluxo,
/// coordenar chamadas ao domínio e persistir alterações por meio do repositório.
/// </remarks>
public class UserApp : IUserApp
{
    /// <summary>
    /// Repositório responsável pelo acesso e persistência dos usuários.
    /// </summary>
    private readonly IUserRepo _userRepo;

    /// <summary>
    /// Serviço responsável por gerar e validar hashes de senha.
    /// </summary>
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="UserApp"/>.
    /// </summary>
    /// <param name="userRepo">Repositório de usuários.</param>
    /// <param name="passwordHasher">Serviço de hash de senha.</param>
    public UserApp(IUserRepo userRepo, IPasswordHasher passwordHasher)
    {
        _userRepo = userRepo;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Adiciona um novo usuário ao sistema.
    /// </summary>
    /// <param name="user">Entidade de usuário a ser cadastrada.</param>
    /// <param name="password">Senha em texto puro informada no cadastro.</param>
    /// <returns>Retorna o identificador do usuário criado.</returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando os dados do usuário ou a senha são inválidos,
    /// ou quando já existe um usuário com o mesmo e-mail.
    /// </exception>
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

    /// <summary>
    /// Obtém um usuário pelo seu identificador.
    /// </summary>
    /// <param name="idUser">ID do usuário.</param>
    /// <returns>Usuário encontrado.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o usuário não é localizado.
    /// </exception>
    public async Task<User> GetByIdAsync(int idUser)
    {
        return await ValidateUserExistsByIdAsync(idUser);
    }

    /// <summary>
    /// Obtém um usuário pelo e-mail.
    /// </summary>
    /// <param name="emailUser">E-mail do usuário.</param>
    /// <returns>Usuário encontrado.</returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando o e-mail não é informado.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o usuário não é localizado.
    /// </exception>
    public async Task<User> GetByEmailAsync(string emailUser)
    {
        if (string.IsNullOrWhiteSpace(emailUser))
            throw new ArgumentException("Email não pode ser vazio");

        var userEntity = await _userRepo.GetByEmailAsync(emailUser);
        if (userEntity == null)
            throw new KeyNotFoundException("Usuário não localizado.");

        return userEntity;
    }

    /// <summary>
    /// Busca usuários cujo nome contenha o valor informado.
    /// </summary>
    /// <param name="nameUser">
    /// Texto utilizado para filtrar os usuários pelo nome.
    /// Não pode ser nulo, vazio ou composto apenas por espaços.
    /// </param>
    /// <returns>
    /// Uma coleção de usuários que possuem o nome contendo o valor informado.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando o parâmetro <paramref name="nameUser"/> é nulo ou inválido.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando nenhum usuário é encontrado para o critério informado.
    /// </exception>
    /// <remarks>
    /// Este método aplica validações de entrada e garante que o resultado da busca não seja vazio.
    /// A responsabilidade de acesso a dados é delegada ao repositório.
    /// </remarks>
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

    /// <summary>
    /// Obtém todos os usuários cadastrados.
    /// </summary>
    /// <returns>Coleção com todos os usuários.</returns>
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userRepo.GetAllAsync();
    }

    /// <summary>
    /// Obtém todos os usuários filtrando pelo status.
    /// </summary>
    /// <param name="statusUser">
    /// Status desejado para o filtro (true = ativo, false = inativo).
    /// </param>
    /// <returns>Coleção de usuários com o status informado.</returns>
    public async Task<IEnumerable<User>> GetAllByStatusAsync(bool statusUser)
    {
        return await _userRepo.GetAllByStatusAsync(statusUser);
    }

    /// <summary>
    /// Atualiza os dados de um usuário existente.
    /// </summary>
    /// <param name="user">Usuário com os dados atualizados.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando os dados do usuário são inválidos
    /// ou quando já existe outro usuário com o mesmo e-mail.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o usuário a ser atualizado não é localizado.
    /// </exception>
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

    /// <summary>
    /// Atualiza a senha de um usuário.
    /// </summary>
    /// <param name="userId">ID do usuário.</param>
    /// <param name="currentPassword">Senha atual informada para validação.</param>
    /// <param name="newPassword">Nova senha que será definida.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando a senha atual ou a nova senha não são informadas.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// Lançada quando a senha atual informada é inválida.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o usuário não é localizado.
    /// </exception>
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

    /// <summary>
    /// Remove um usuário do sistema.
    /// </summary>
    /// <param name="idUser">ID do usuário a ser removido.</param>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o usuário não é localizado.
    /// </exception>
    /// <remarks>
    /// Este método realiza remoção física do registro.
    /// </remarks>
    public async Task DeleteAsync(int idUser)
    {
        var userEntity = await ValidateUserExistsByIdAsync(idUser);

        await _userRepo.DeleteAsync(userEntity);
    }

    /// <summary>
    /// Desativa um usuário no sistema.
    /// </summary>
    /// <param name="idUser">ID do usuário a ser desativado.</param>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o usuário não é localizado.
    /// </exception>
    public async Task DeactivateAsync(int idUser)
    {
        var userEntity = await ValidateUserExistsByIdAsync(idUser);

        userEntity.Deactivate();

        await _userRepo.UpdateAsync(userEntity);
    }

    /// <summary>
    /// Ativa um usuário no sistema.
    /// </summary>
    /// <param name="idUser">ID do usuário a ser ativado.</param>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o usuário não é localizado.
    /// </exception>
    public async Task ActivateAsync(int idUser)
    {
        var userEntity = await ValidateUserExistsByIdAsync(idUser);

        userEntity.Activate();

        await _userRepo.UpdateAsync(userEntity);
    }


    #region Métodos auxiliares

    /// <summary>
    /// Valida as informações básicas obrigatórias de um usuário.
    /// </summary>
    /// <param name="user">Usuário a ser validado.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando o usuário, nome, e-mail ou tipo de usuário são inválidos.
    /// </exception>
    private static void ValidateUserInformation(User user)
    {
        if (user == null)
            throw new ArgumentException("Usuário não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(user.Name))
            throw new ArgumentException("O nome do usuário deve ser informado.");

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("O e-mail do usuário deve ser informado.");
    }

    /// <summary>
    /// Valida se existe um usuário com o ID informado.
    /// </summary>
    /// <param name="idUser">ID do usuário.</param>
    /// <returns>Usuário encontrado.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o usuário não é localizado.
    /// </exception>
    private async Task<User> ValidateUserExistsByIdAsync(int idUser)
    {
        var userEntity = await _userRepo.GetByIdAsync(idUser);
        if (userEntity == null)
            throw new KeyNotFoundException("Usuário não localizado.");

        return userEntity;
    }

    #endregion
}