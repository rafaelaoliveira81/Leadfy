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
    public async Task<IEnumerable<Lead>> GetByNameContainingAsync(string nameLead)
    {
        return await _context.Leads
            .Where(l => EF.Functions.Like(l.Name, $"%{nameLead}%"))
            .ToListAsync();
    }
    public async Task<IEnumerable<Lead>> GetAllAsync()
    {
        return await _context.Leads.ToListAsync();
    }
    public async Task<IEnumerable<Lead>> GetAllByStatusAsync(bool statusLead)
    {
        var query = "sp_GetAllLeads";
        var parameters = new { IsActive = statusLead };

        using (var connection = GetConnection())
        {
            return await connection.QueryAsync<Lead>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
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
