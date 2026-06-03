using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Repository.Context;

namespace Repository.Repositories;

public class OpportunityActionPlanRepo : BaseRepo, IOpportunityActionPlanRepo
{
    public OpportunityActionPlanRepo(CRMContext context) : base(context)
    {
    }

    public async Task<int> AddAsync(OpportunityActionPlan actionPlan)
    {
        var query = "sp_CreateOpportunityActionPlan";
        var parameters = new
        {
            OpportunityId = actionPlan.OpportunityId,
            Message = actionPlan.Message,
            ActionPlan = actionPlan.ActionPlan,
            GeneratedAt = actionPlan.GeneratedAt
        };

        using (var connection = GetConnection())
        {
            return await connection.ExecuteScalarAsync<int>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }

    public async Task<OpportunityActionPlan> GetByIdAsync(int id)
    {
        var query = "sp_GetOpportunityActionPlanById";
        var parameters = new { ID = id };

        using (var connection = GetConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<OpportunityActionPlan>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }

    public async Task<IEnumerable<OpportunityActionPlan>> GetByOpportunityIdAsync(int opportunityId)
    {
        return await _context.OpportunityActionPlans
            .Where(ap => ap.OpportunityId == opportunityId)
            .OrderByDescending(ap => ap.GeneratedAt)
            .ToListAsync();
    }
}
