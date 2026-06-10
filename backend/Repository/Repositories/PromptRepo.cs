using Microsoft.EntityFrameworkCore;
using Dapper;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

public class PromptRepo : BaseRepository<Prompt>, IPromptRepo
{
    public PromptRepo(CRMContext context) : base(context)
    {
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
}