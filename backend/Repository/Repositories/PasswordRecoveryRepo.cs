using Microsoft.EntityFrameworkCore;
using Dapper;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

/// <summary>
/// Repository responsible for persisting and querying password recovery requests.
/// The Application layer owns the business rules (expiration, single-use, cooldown).
/// Contains only data access logic — no business rules.
/// </summary>
public class PasswordRecoveryRepo : BaseRepo, IPasswordRecoveryRepo
{
    public PasswordRecoveryRepo(CRMContext context) : base(context)
    {
    }

    public async Task<int> AddAsync(PasswordRecovery passwordRecovery)
    {
        var query = "sp_CreatePasswordRecovery";
        var parameters = new
        {
            UserId = passwordRecovery.UserId,
            Email = passwordRecovery.Email,
            Token = passwordRecovery.Token,
            ExpiresAt = passwordRecovery.ExpiresAt
        };

        using (var connection = GetConnection())
        {
            return await connection.ExecuteScalarAsync<int>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }

    public async Task<PasswordRecovery> GetByIdAsync(int id)
    {
        var query = "sp_GetPasswordRecoveryById";
        var parameters = new { ID = id };

        using (var connection = GetConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<PasswordRecovery>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }

    /// <summary>
    /// Returns the recovery record for the given token.
    /// Active and inactive records are both returned so the Application layer can
    /// detect already-consumed tokens and return appropriate error messages.
    /// </summary>
    public async Task<PasswordRecovery> GetByTokenAsync(string token)
    {
        return await _context.PasswordRecoveries
            .FirstOrDefaultAsync(pr => pr.Token == token);
    }

    /// <summary>
    /// Returns the most recent recovery request for the given email.
    /// Used to enforce the minimum interval between password recovery requests.
    /// </summary>
    public async Task<PasswordRecovery> GetLatestByEmailAsync(string email)
    {
        return await _context.PasswordRecoveries
            .Where(pr => pr.Email == email)
            .OrderByDescending(pr => pr.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(PasswordRecovery passwordRecovery)
    {
        var query = "sp_UpdatePasswordRecovery";
        var parameters = new
        {
            ID = passwordRecovery.ID,
            UserId = passwordRecovery.UserId,
            Email = passwordRecovery.Email,
            Token = passwordRecovery.Token,
            ExpiresAt = passwordRecovery.ExpiresAt,
            IsActive = passwordRecovery.IsActive
        };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }

    public async Task DeleteAsync(PasswordRecovery passwordRecovery)
    {
        var query = "sp_DeletePasswordRecovery";
        var parameters = new { ID = passwordRecovery.ID };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
