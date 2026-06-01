using Microsoft.EntityFrameworkCore;
using Dapper;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

public class PromptRepo : BaseRepo, IPromptRepo
{
    public PromptRepo(CRMContext context) : base(context)
    {
    }
    public async Task<int> AddAsync(Prompt prompt)
    {
        var query = "sp_CreatePrompt";
        var parameters = new
        {
            Title = prompt.Title,
            Content = prompt.Content,
            IsActive = prompt.IsActive
        };

        using (var connection = GetConnection())
        {
            return await connection.ExecuteScalarAsync<int>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task<Prompt> GetByIdAsync(int idPrompt)
    {
        var query = "sp_GetPromptById";
        var parameters = new { ID = idPrompt };

        using (var connection = GetConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Prompt>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }

    public async Task<PagedResult<Prompt>> GetPagedAsync(
        bool? isActive,
        int pagina,
        int quantidadePorPagina)
    {
        using var connection = GetConnection();

        using var multi = await connection.QueryMultipleAsync(
            "sp_GetPromptsPaginado",
            new
            {
                Status = isActive.HasValue ? (isActive.Value ? 1 : 0) : (int?)null,
                Pagina = pagina,
                QuantidadePorPagina = quantidadePorPagina
            },
            commandType: System.Data.CommandType.StoredProcedure
        );

        var totalRegistros = await multi.ReadFirstAsync<int>();
        var prompts = (await multi.ReadAsync<Prompt>()).ToList();

        return new PagedResult<Prompt>
        {
            TotalRegistros = totalRegistros,
            Dados = prompts
        };
    }
    public async Task UpdateAsync(Prompt prompt)
    {
        var query = "sp_UpdatePrompt";
        var parameters = new
        {
            ID = prompt.Id,
            Title = prompt.Title,
            Content = prompt.Content,
            IsActive = prompt.IsActive
        };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }

    public async Task DeleteAsync(Prompt prompt)
    {
        var query = "sp_DeletePrompt";
        var parameters = new { ID = prompt.Id };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}