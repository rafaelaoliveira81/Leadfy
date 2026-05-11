using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

/// <summary>
/// Repositório responsável pela persistência e consulta de usuários.
/// Implementa operações CRUD utilizando Entity Framework Core.
/// </summary>
/// <remarks>
/// Esta classe pertence à camada de Repository e deve conter apenas
/// lógica de acesso a dados, sem regras de negócio.
/// </remarks>
public class UserRepository : BaseRepo, IUserRepo
{
    public UserRepository(CRMContext context) : base(context)
    {
    }

    /// <summary>
    /// Adiciona um novo usuário no banco de dados.
    /// </summary>
    /// <param name="user">Entidade do usuário a ser persistida.</param>
    /// <returns>Retorna o ID do usuário gerado após a inserção.</returns>
    public async Task<int> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user.ID;
    }

    /// <summary>
    /// Busca um usuário pelo seu identificador único.
    /// </summary>
    /// <param name="idUser">ID do usuário.</param>
    /// <returns>Usuário encontrado ou null caso não exista.</returns>
    public async Task<User> GetByIdAsync(int idUser)
    {
        return await _context.Users.FindAsync(idUser);
    }

    /// <summary>
    /// Busca um usuário pelo email.
    /// </summary>
    /// <param name="emailUser">Email do usuário.</param>
    /// <returns>Usuário correspondente ou null.</returns>
    /// <remarks>
    /// Ideal para autenticação e validação de unicidade.
    /// </remarks>
    public async Task<User> GetByEmailAsync(string emailUser)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == emailUser);
    }

    /// <summary>
    /// Busca usuários cujo nome contenha o valor informado.
    /// </summary>
    /// <param name="nameUser">
    /// Texto utilizado para filtrar os usuários pelo nome.
    /// A busca é case-insensitive.
    /// </param>
    /// <returns>
    /// Uma coleção de usuários que possuem o nome contendo o valor informado.
    /// Retorna uma lista vazia caso nenhum usuário seja encontrado.
    /// </returns>
    public async Task<IEnumerable<User>> GetByNameContainingAsync(string nameUser)
    {
        return await _context.Users
            .Where(u => EF.Functions.Like(u.Name, $"%{nameUser}%"))
            .ToListAsync();
    }

    /// <summary>
    /// Retorna todos os usuários cadastrados.
    /// </summary>
    /// <returns>Lista de usuários.</returns>
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    /// <summary>
    /// Retorna todos os usuários filtrando pelo status (ativo/inativo).
    /// </summary>
    /// <param name="statusUser">Status do usuário (true = ativo, false = inativo).</param>
    /// <returns>Lista de usuários filtrados.</returns>
    public async Task<IEnumerable<User>> GetAllByStatusAsync(bool statusUser)
    {
        return await _context.Users
            .Where(u => u.IsActive == statusUser)
            .ToListAsync();
    }

    /// <summary>
    /// Atualiza os dados de um usuário existente.
    /// </summary>
    /// <param name="user">Usuário com dados atualizados.</param>
    /// <remarks>
    /// O Entity Framework irá rastrear as alterações e persistir no banco.
    /// </remarks>
    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove um usuário do banco de dados.
    /// </summary>
    /// <param name="user">Usuário a ser removido.</param>
    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Returns the user with UserGroup, UserGroupPermissions and Permission loaded.
    /// Use for JWT login — builds the full graph needed for claim generation in one query.
    /// Returns null when no active user matches the email.
    /// </summary>
    public async Task<User> GetByEmailWithGroupAsync(string emailUser)
    {
        return await _context.Users
            .Include(u => u.UserGroup)
                .ThenInclude(g => g.UserGroupPermissions)
                    .ThenInclude(ugp => ugp.Permission)
            .FirstOrDefaultAsync(u => u.Email == emailUser && u.IsActive);
    }

    /// <summary>
    /// Returns an active user by ID. Returns null when the user is inactive.
    /// </summary>
    public async Task<User> GetActiveByIdAsync(int idUser)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.ID == idUser && u.IsActive);
    }
}