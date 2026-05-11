using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

/// <summary>
/// Repository responsible for persisting and querying permissions.
/// Contains only data access logic — no business rules.
/// </summary>
public class PermissionRepo : BaseRepo, IPermissionRepo
{
    public PermissionRepo(CRMContext context) : base(context)
    {
    }

    public async Task<int> AddAsync(Permission permission)
    {
        _context.Permissions.Add(permission);
        await _context.SaveChangesAsync();
        return permission.ID;
    }

    public async Task<Permission> GetByIdAsync(int id)
    {
        return await _context.Permissions.FindAsync(id);
    }

    public async Task<Permission> GetByNameAsync(string name)
    {
        return await _context.Permissions
            .FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<IEnumerable<Permission>> GetAllAsync()
    {
        return await _context.Permissions.ToListAsync();
    }

    public async Task<IEnumerable<Permission>> GetAllActiveAsync()
    {
        return await _context.Permissions
            .Where(p => p.IsActive)
            .ToListAsync();
    }

    public async Task UpdateAsync(Permission permission)
    {
        _context.Permissions.Update(permission);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Permission permission)
    {
        _context.Permissions.Remove(permission);
        await _context.SaveChangesAsync();
    }
}
