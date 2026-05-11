using Domain.Enuns;

namespace Domain.Entities;

/// <summary>
/// Representa um usuário do sistema.
/// Entidade central do domínio responsável por autenticação e controle de acesso.
/// </summary>
public class User
{    public int ID { get; set; } //required
    public string Name { get; set; } //required
    public string Email { get; set; } //required and unique
    public string PasswordHash { get; private set; } //required
    public UserRole Role { get; set; } //required
    public bool IsActive { get; set; } //required and default true
    public DateTime CreatedAt { get; set; } //required and default current time

    /// <summary>
    /// Optional FK to UserGroup. Null for users migrated before group-based auth was introduced.
    /// UserRole is kept for backward compatibility during transition.
    /// </summary>
    public int? UserGroupId { get; set; }
    public UserGroup UserGroup { get; set; }

    public ICollection<Ower> Owers { get; set; } //Relacionamento 1:N com Ower
    public ICollection<Interaction> Interactions { get; set; } //Relacionamento 1:N com Interaction
    public ICollection<PasswordRecovery> PasswordRecoveries { get; set; } //Relacionamento 1:N com PasswordRecovery
    
    /// <summary>
    /// Construtor padrão.
    /// Inicializa o usuário como ativo e define a data de criação.
    /// </summary>
    public User()
    {
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        Owers = new List<Ower>();
        Interactions = new List<Interaction>();
        PasswordRecoveries = new List<PasswordRecovery>();
    }

    /// <summary>
    /// Define a senha do usuário utilizando um serviço de hash.
    /// </summary>
    /// <param name="password">Senha em texto puro.</param>
    /// <param name="hasher">Serviço responsável por gerar o hash.</param>
    public void SetPassword(string password, IPasswordHasher hasher)
    {
        PasswordHash = hasher.Hash(password);
    }

    /// <summary>
    /// Verifica se a senha informada corresponde ao hash armazenado.
    /// </summary>
    /// <param name="password">Senha informada pelo usuário.</param>
    /// <param name="hasher">Serviço de verificação de hash.</param>
    /// <returns>True se a senha for válida; caso contrário, false.</returns>
    public bool VerifyPassword(string password, IPasswordHasher hasher)
    {
        return hasher.Verify(PasswordHash, password);
    }

    /// <summary>
    /// Altera a senha do usuário.
    /// </summary>
    /// <param name="currentPassword">Senha atual (não validada aqui pois valida no layer de Application).</param>
    /// <param name="newPassword">Nova senha.</param>
    /// <param name="hasher">Serviço de hash.</param>
    /// <remarks>
    /// Regra importante: a validação da senha atual deve ser feita antes,
    /// preferencialmente na camada de Application.
    /// </remarks>
    public void ChangePassword(string currentPassword, string newPassword, IPasswordHasher hasher)
    {    
        PasswordHash = hasher.Hash(newPassword);
    }

    /// <summary>
    /// Desativa o usuário no sistema.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// Ativa o usuário no sistema.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }
}