using Microsoft.EntityFrameworkCore;
using Dapper;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

public class OpportunityRepo : BaseRepository<Opportunity>, IOpportunityRepo
{
    public OpportunityRepo(CRMContext context) : base(context)
    {
    }
    public async Task<IEnumerable<Opportunity>> GetByStageAsync(Guid tenantId, int stage)
    {
        return await _context.Opportunities
            .Where(o => o.TenantId == tenantId && (int)o.Stage == stage)
            .Include(o => o.Lead)
            .Where(o => o.Lead.IsActive)
            .Include(o => o.Product)
            .OrderBy(o => o.SortOrder)
            .ToListAsync();
    }
}
