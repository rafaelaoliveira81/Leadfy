using Microsoft.EntityFrameworkCore;
using Dapper;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

public class OwnerRepo : BaseRepo, IOwnerRepo
{
    public OwnerRepo(CRMContext context) : base(context)
    {
    }
    public async Task<int> AddAsync(Owner owner)
    {
        var query = "sp_CreateOwner";
        var parameters = new
        {
            Name = owner.Name,
            UserID = owner.UserID
        };

        using (var connection = GetConnection())
        {
            return await connection.ExecuteScalarAsync<int>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task<Owner> GetByIdAsync(int idOwner)
    {
        var query = "sp_GetOwnerById";
        var parameters = new { ID = idOwner };

        using (var connection = GetConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Owner>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task<IEnumerable<Owner>> GetAllAsync(bool? statusOwner)
    {
        var query = "sp_GetAllOwners";
        var parameters = new { IsActive = statusOwner };

        using (var connection = GetConnection())
        {
            return await connection.QueryAsync<Owner>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task UpdateAsync(Owner owner)
    {
        var query = "sp_UpdateOwner";
        var parameters = new
        {
            ID = owner.ID,
            Name = owner.Name,
            UserID = owner.UserID,
            IsActive = owner.IsActive
        };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task DeleteAsync(Owner owner)
    {
        var query = "sp_DeleteOwner";
        var parameters = new { ID = owner.ID };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}