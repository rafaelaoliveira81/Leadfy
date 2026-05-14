using Microsoft.EntityFrameworkCore;
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
        _context.Opportunities.Add(opportunity);
        await _context.SaveChangesAsync();

        return opportunity.ID;
    }
    public async Task<Opportunity> GetByIdAsync(int idOpportunity)
    {
        return await _context.Opportunities
            .Include(o => o.Lead)
            .Include(o => o.Owner)
            .Include(o => o.Product)
            .FirstOrDefaultAsync(o => o.ID == idOpportunity);
    }
    public async Task<IEnumerable<Opportunity>> GetAllAsync()
    {
        return await _context.Opportunities
            .Include(o => o.Lead)
            .Include(o => o.Owner)
            .Include(o => o.Product)
            .OrderBy(o => o.Stage)
            .ThenBy(o => o.SortOrder)
            .ToListAsync();
    }
    public async Task<IEnumerable<Opportunity>> GetAllByStatusAsync(bool statusOpportunity)
    {
        return await _context.Opportunities
            .Where(o => o.IsActive == statusOpportunity)
            .Include(o => o.Lead)
            .Include(o => o.Owner)
            .Include(o => o.Product)
            .OrderBy(o => o.Stage)
            .ThenBy(o => o.SortOrder)
            .ToListAsync();
    }
    public async Task<IEnumerable<Opportunity>> GetByLeadIdAsync(int leadId)
    {
        return await _context.Opportunities
            .Where(o => o.LeadId == leadId)
            .Include(o => o.Lead)
            .Include(o => o.Owner)
            .Include(o => o.Product)
            .ToListAsync();
    }
    public async Task<IEnumerable<Opportunity>> GetByOwnerIdAsync(int ownerId)
    {
        return await _context.Opportunities
            .Where(o => o.OwnerId == ownerId)
            .Include(o => o.Lead)
            .Include(o => o.Owner)
            .Include(o => o.Product)
            .ToListAsync();
    }
    public async Task UpdateAsync(Opportunity opportunity)
    {
        _context.Opportunities.Update(opportunity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Opportunity opportunity)
    {
        _context.Opportunities.Remove(opportunity);
        await _context.SaveChangesAsync();
    }
}
