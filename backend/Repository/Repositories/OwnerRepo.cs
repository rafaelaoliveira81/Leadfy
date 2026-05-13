using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

public class OwnerRepo : BaseRepo, IOwnerRepo
{
    public OwnerRepo(CRMContext context) : base(context)
    {
    }
    public async Task<int> AddAsync(Owner owner)
    {
        _context.Owners.Add(owner);
        await _context.SaveChangesAsync();

        return owner.ID;
    }
    public async Task<Owner> GetByIdAsync(int idOwner)
    {
        return await _context.Owners
            .Include(u => u.User)
            .FirstOrDefaultAsync(o => o.ID == idOwner);
    }
    public async Task<IEnumerable<Owner>> GetByNameContainingAsync(string nameOwner)
    {
        return await _context.Owners
            .Where(o => EF.Functions.Like(o.Name, $"%{nameOwner}%"))
            .Include(u => u.User)
            .ToListAsync();
    }
    public async Task<IEnumerable<Owner>> GetAllAsync()
    {
        return await _context.Owners
            .Include(u => u.User)
            .ToListAsync();
    }
    public async Task<IEnumerable<Owner>> GetAllByStatusAsync(bool statusOwner)
    {
        return await _context.Owners
            .Where(o => o.IsActive == statusOwner)
            .Include(u => u.User)
            .ToListAsync();
    }
    public async Task UpdateAsync(Owner owner)
    {
        _context.Owners.Update(owner);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Owner owner)
    {
        _context.Owners.Remove(owner);
        await _context.SaveChangesAsync();
    }
}