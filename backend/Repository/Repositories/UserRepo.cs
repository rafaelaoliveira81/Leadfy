using Microsoft.EntityFrameworkCore;
using Dapper;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

public class UserRepository : BaseRepo, IUserRepo
{
    public UserRepository(CRMContext context) : base(context)
    {
    }

    public async Task<int> AddAsync(User user)
    {
        var query = "sp_CreateUser";
        var parameters = new
        {
            Name = user.Name,
            Email = user.Email,
            PasswordHash = user.PasswordHash
        };

        using (var connection = GetConnection())
        {
            return await connection.ExecuteScalarAsync<int>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task<User> GetByIdAsync(int idUser)
    {
        var query = "sp_GetUserById";
        var parameters = new { ID = idUser };

        using (var connection = GetConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<User>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task<User> GetByEmailAsync(string emailUser)
    {
        var query = "sp_GetUserByEmail";
        var parameters = new { Email = emailUser };

        using (var connection = GetConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<User>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }

    public async Task<PagedResult<User>> GetPagedAsync(
        bool? isActive,
        int pagina,
        int quantidadePorPagina)
    {
        using var connection = GetConnection();

        using var multi = await connection.QueryMultipleAsync(
            "sp_GetUsersPaginado",
            new
            {
                isActive = isActive.HasValue ? (isActive.Value ? 1 : 0) : (int?)null,
                Pagina = pagina,
                QuantidadePorPagina = quantidadePorPagina
            },
            commandType: System.Data.CommandType.StoredProcedure
        );

        var totalRegistros = await multi.ReadFirstAsync<int>();
        var users = (await multi.ReadAsync<User>()).ToList();

        return new PagedResult<User>
        {
            TotalRegistros = totalRegistros,
            Dados = users
        };
    }
    public async Task<IEnumerable<User>> GetAllByStatusAsync(bool statusUser)
    {
        var query = "sp_GetAllUsers";
        var parameters = new { IsActive = statusUser };

        using (var connection = GetConnection())
        {
            return await connection.QueryAsync<User>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task UpdateAsync(User user)
    {
        var query = "sp_UpdateUser";
        var parameters = new
        {
            ID = user.ID,
            Name = user.Name,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            IsActive = user.IsActive
        };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task DeleteAsync(User user)
    {
        var query = "sp_DeleteUser";
        var parameters = new { ID = user.ID };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task<User> GetByEmailWithGroupAsync(string emailUser)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == emailUser && u.IsActive);
    }

    public async Task<User> GetActiveByIdAsync(int idUser)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.ID == idUser && u.IsActive);
    }
}