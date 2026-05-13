using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

public class OwerRepo : BaseRepo, IOwerRepo
{
    public OwerRepo(CRMContext context) : base(context)
    {
    }
    public async Task<int> AddAsync(Ower ower)
    {
        _context.Owers.Add(ower);
        await _context.SaveChangesAsync();

        return ower.ID;
    }
    public async Task<Ower> GetByIdAsync(int idOwer)
    {
        return await _context.Owers
            .Include(u => u.User)
            .FirstOrDefaultAsync(o => o.ID == idOwer);
    }
    public async Task<IEnumerable<Ower>> GetByNameContainingAsync(string nameOwer)
    {
        return await _context.Owers
            .Where(o => EF.Functions.Like(o.Name, $"%{nameOwer}%"))
            .Include(u => u.User)
            .ToListAsync();
    }
    public async Task<IEnumerable<Ower>> GetAllAsync()
    {
        return await _context.Owers
            .Include(u => u.User)
            .ToListAsync();
    }
    public async Task<IEnumerable<Ower>> GetAllByStatusAsync(bool statusOwer)
    {
        return await _context.Owers
            .Where(o => o.IsActive == statusOwer)
            .Include(u => u.User)
            .ToListAsync();
    }
    public async Task UpdateAsync(Ower ower)
    {
        _context.Owers.Update(ower);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Ower ower)
    {
        _context.Owers.Remove(ower);
        await _context.SaveChangesAsync();
    }
}