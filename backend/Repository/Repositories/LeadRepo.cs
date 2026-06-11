using Microsoft.EntityFrameworkCore;
using Dapper;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

public class LeadRepo : BaseRepository<Lead>, ILeadRepo
{
    public LeadRepo(CRMContext context) : base(context)
    {
    }
    public async Task<PagedResult<Lead>> GetPagedAsync(
        Guid tenantId,
        bool? isActive,
        int pagina,
        int quantidadePorPagina)
    {
        using var connection = GetConnection();

        using var multi = await connection.QueryMultipleAsync(
            "sp_GetLeadsPaginado",
            new
            {
                TenantId = tenantId,
                Status = isActive.HasValue ? (isActive.Value ? 1 : 0) : (int?)null,
                Pagina = pagina,
                QuantidadePorPagina = quantidadePorPagina
            },
            commandType: System.Data.CommandType.StoredProcedure
        );

        var totalRegistros = await multi.ReadFirstAsync<int>();
        var leads = (await multi.ReadAsync<Lead>()).ToList();

        return new PagedResult<Lead>
        {
            TotalRegistros = totalRegistros,
            Dados = leads
        };
    }
}
