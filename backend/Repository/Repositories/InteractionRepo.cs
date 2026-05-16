using Domain.Entities;
using Domain.Enuns;
using Microsoft.EntityFrameworkCore;
using Repository.Context;

namespace Repository.Repositories;

public class InteractionRepo : BaseRepo, IInteractionRepo
{
    public InteractionRepo(CRMContext context) : base(context)
    {
    }
    public async Task<int> AddAsync(Interaction interaction)
    {
        _context.Interactions.Add(interaction);
        await _context.SaveChangesAsync();

        return interaction.Id;
    }
    public async Task<Interaction> GetByIdAsync(int idInteraction)
    {
        return await _context.Interactions
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.Id == idInteraction);
    }
    public async Task<IEnumerable<Interaction>> GetByCrmEntityAsync(CrmEntityType crmEntityType, int crmEntityId)
    {
        return await _context.Interactions
            .Where(i => i.CrmEntityType == crmEntityType && i.CrmEntityId == crmEntityId)
            .Include(i => i.User)
            .OrderByDescending(i => i.InteractionDate)
            .ThenByDescending(i => i.CreatedAt)
            .ToListAsync();
    }
    public async Task<IEnumerable<Interaction>> GetAllByOpportunityIdAsync(int opportunityId)
    {
        return await GetByCrmEntityAsync(CrmEntityType.Opportunity, opportunityId);
    }

    public async Task<IEnumerable<Interaction>> GetLastInteractionsByOpportunityIdAsync(int opportunityId)
    {
        return await _context.Interactions
            .Where(i =>
                i.CrmEntityType == CrmEntityType.Opportunity &&
                i.CrmEntityId == opportunityId)
            .Include(i => i.User)
            .OrderByDescending(i => i.InteractionDate)
            .ThenByDescending(i => i.CreatedAt)
            .Take(3)
            .ToListAsync();
    }

    public async Task UpdateAsync(Interaction interaction)
    {
        _context.Interactions.Update(interaction);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Interaction interaction)
    {
        _context.Interactions.Remove(interaction);
        await _context.SaveChangesAsync();
    }
}