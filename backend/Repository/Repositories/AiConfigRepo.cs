using Microsoft.EntityFrameworkCore;
using Dapper;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

public class AiConfigRepo : BaseRepo, IAiConfigRepo
{
    public AiConfigRepo(CRMContext context) : base(context) { }

    public async Task<int> AddAsync(AiConfig config)
    {
        var query = "sp_CreateAiConfig";
        var parameters = new
        {
            Title = config.Title,
            PromptTemplate = config.PromptTemplate,
            ApiKeyHash = config.ApiKeyHash,
            Model = (int)config.Model
        };

        using (var connection = GetConnection())
        {
            return await connection.ExecuteScalarAsync<int>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }

    public async Task<AiConfig> GetByIdAsync(int id)
    {
        var query = "sp_GetAiConfigById";
        var parameters = new { ID = id };

        using (var connection = GetConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<AiConfig>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }

    public async Task<AiConfig> GetActiveAsync()
    {
        return await _context.AiConfigs
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<AiConfig>> GetAllAsync()
    {
        var query = "sp_GetAllAiConfigs";
        var parameters = new { IsActive = (bool?)null };

        using (var connection = GetConnection())
        {
            return await connection.QueryAsync<AiConfig>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task UpdateAsync(AiConfig config)
    {
        var query = "sp_UpdateAiConfig";
        var parameters = new
        {
            ID = config.Id,
            Title = config.Title,
            PromptTemplate = config.PromptTemplate,
            ApiKeyHash = config.ApiKeyHash,
            Model = (int)config.Model,
            IsActive = config.IsActive
        };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }

    public async Task DeleteAsync(AiConfig config)
    {
        var query = "sp_DeleteAiConfig";
        var parameters = new { ID = config.Id };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
