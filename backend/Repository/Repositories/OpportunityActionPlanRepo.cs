using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Repository.Context;

namespace Repository.Repositories;

public class OpportunityActionPlanRepo : BaseRepository<OpportunityActionPlan>, IOpportunityActionPlanRepo
{
    public OpportunityActionPlanRepo(CRMContext context) : base(context)
    {
    }

    public async Task<IEnumerable<OpportunityActionPlan>> GetByOpportunityIdAsync(Guid opportunityId)
    {
        return await _context.OpportunityActionPlans
            .Where(ap => ap.OpportunityId == opportunityId)
            .OrderByDescending(ap => ap.GeneratedAt)
            .ToListAsync();
    }
}
