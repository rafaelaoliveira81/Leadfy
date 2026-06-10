using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Repository.Context;

namespace Repository.Repositories;

public class InteractionRepo : BaseRepository<Interaction>, IInteractionRepo
{
    public InteractionRepo(CRMContext context) : base(context)
    {
    }
    public async Task<IEnumerable<Interaction>> GetAllByOpportunityIdAsync(Guid opportunityId)
    {
        return await _context.Interactions
            .Where(i => i.OpportunityId == opportunityId)
            .Include(i => i.User)
            .OrderByDescending(i => i.InteractionDate)
            .ThenByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Interaction>> GetLastInteractionsByOpportunityIdAsync(Guid opportunityId)
    {
        return await _context.Interactions
            .Where(i => i.OpportunityId == opportunityId)
            .Include(i => i.User)
            .OrderByDescending(i => i.InteractionDate)
            .ThenByDescending(i => i.CreatedAt)
            .Take(3)
            .ToListAsync();
    }
}