using Microsoft.EntityFrameworkCore;
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
        _context.Leads.Add(lead);
        await _context.SaveChangesAsync();

        return lead.Id;
    }
    public async Task<Lead> GetByIdAsync(int idLead)
    {
        return await _context.Leads
            .FirstOrDefaultAsync(l => l.Id == idLead);
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
        return await _context.Leads
            .Where(l => l.IsActive == statusLead)
            .ToListAsync();
    }
    public async Task UpdateAsync(Lead lead)
    {
        _context.Leads.Update(lead);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Lead lead)
    {
        _context.Leads.Remove(lead);
        await _context.SaveChangesAsync();
    }
}
