using Microsoft.EntityFrameworkCore;
using Dapper;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

public class OpportunityRepo : BaseRepo, IOpportunityRepo
{
    public OpportunityRepo(CRMContext context) : base(context)
    {
    }
    public async Task<int> AddAsync(Opportunity opportunity)
    {
        var query = "sp_CreateOpportunity";
        var parameters = new
        {
            LeadId = opportunity.LeadId,
            ProductId = opportunity.ProductId,
            Stage = (int)opportunity.Stage,
            Amount = opportunity.Amount,
            ExpectedCloseDate = opportunity.ExpectedCloseDate,
            SortOrder = opportunity.SortOrder
        };

        using (var connection = GetConnection())
        {
            return await connection.ExecuteScalarAsync<int>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task<Opportunity> GetByIdAsync(int idOpportunity)
    {
        var query = "sp_GetOpportunityById";
        var parameters = new { ID = idOpportunity };

        using (var connection = GetConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Opportunity>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task<IEnumerable<Opportunity>> GetAllAsync()
    {
        return await _context.Opportunities
            .Include(o => o.Lead)
            .Include(o => o.Product)
            .OrderBy(o => o.Stage)
            .ThenBy(o => o.SortOrder)
            .ToListAsync();
    }
    public async Task<IEnumerable<Opportunity>> GetAllByStatusAsync(bool statusOpportunity)
    {
        var query = "sp_GetAllOpportunities";
        var parameters = new { IsActive = statusOpportunity };

        using (var connection = GetConnection())
        {
            return await connection.QueryAsync<Opportunity>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task<IEnumerable<Opportunity>> GetByStageAsync(int stage)
    {
        return await _context.Opportunities
            .Where(o => (int)o.Stage == stage)
            .Include(o => o.Lead)
            .Include(o => o.Product)
            .OrderBy(o => o.SortOrder)
            .ToListAsync();
    }
    public async Task<IEnumerable<Opportunity>> GetByLeadIdAsync(int leadId)
    {
        return await _context.Opportunities
            .Where(o => o.LeadId == leadId)
            .Include(o => o.Lead)
            .Include(o => o.Product)
            .ToListAsync();
    }
    public async Task UpdateAsync(Opportunity opportunity)
    {
        var query = "sp_UpdateOpportunity";
        var parameters = new
        {
            ID = opportunity.ID,
            LeadId = opportunity.LeadId,
            ProductId = opportunity.ProductId,
            Stage = (int)opportunity.Stage,
            Amount = opportunity.Amount,
            ExpectedCloseDate = opportunity.ExpectedCloseDate,
            IsActive = opportunity.IsActive,
            SortOrder = opportunity.SortOrder
        };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task DeleteAsync(Opportunity opportunity)
    {
        var query = "sp_DeleteOpportunity";
        var parameters = new { ID = opportunity.ID };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
