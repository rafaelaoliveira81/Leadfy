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
    public async Task<IEnumerable<User>> GetByNameContainingAsync(string nameUser)
    {
        return await _context.Users
            .Where(u => EF.Functions.Like(u.Name, $"%{nameUser}%"))
            .ToListAsync();
    }
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
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

    /// <summary>
    /// Returns an active user by email for JWT login.
    /// Returns null when no active user matches the email.
    /// </summary>
    public async Task<User> GetByEmailWithGroupAsync(string emailUser)
    {
        return await _context.Users
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