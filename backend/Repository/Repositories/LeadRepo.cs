using Microsoft.EntityFrameworkCore;
using Dapper;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

public class LeadRepo : BaseRepo, ILeadRepo
{
    public LeadRepo(CRMContext context) : base(context)
    {
    }
    public async Task<int> AddAsync(Lead lead)
    {
        var query = "sp_CreateLead";
        var parameters = new
        {
            Name = lead.Name,
            Email = lead.Email,
            PhoneNumber = lead.PhoneNumber
        };

        using (var connection = GetConnection())
        {
            return await connection.ExecuteScalarAsync<int>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task<Lead> GetByIdAsync(int idLead)
    {
        var query = "sp_GetLeadById";
        var parameters = new { Id = idLead };

        using (var connection = GetConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Lead>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task<PagedResult<Lead>> GetPagedAsync(
        bool? isActive,
        int pagina,
        int quantidadePorPagina)
    {
        using var connection = GetConnection();

        using var multi = await connection.QueryMultipleAsync(
            "sp_GetLeadsPaginado",
            new
            {
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
    public async Task UpdateAsync(Lead lead)
    {
        var query = "sp_UpdateLead";
        var parameters = new
        {
            Id = lead.Id,
            Name = lead.Name,
            Email = lead.Email,
            PhoneNumber = lead.PhoneNumber,
            IsActive = lead.IsActive
        };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task DeleteAsync(Lead lead)
    {
        var query = "sp_DeleteLead";
        var parameters = new { Id = lead.Id };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
