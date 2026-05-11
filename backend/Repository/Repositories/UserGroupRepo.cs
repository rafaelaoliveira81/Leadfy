using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

/// <summary>
/// Repository responsible for persisting and querying user groups.
/// Contains only data access logic — no business rules.
/// </summary>
public class UserGroupRepo : BaseRepo, IUserGroupRepo
{
    public UserGroupRepo(CRMContext context) : base(context)
    {
    }

    public async Task<int> AddAsync(UserGroup userGroup)
    {
        _context.UserGroups.Add(userGroup);
        await _context.SaveChangesAsync();
        return userGroup.ID;
    }

    public async Task<UserGroup> GetByIdAsync(int id)
    {
        return await _context.UserGroups.FindAsync(id);
    }

    public async Task<UserGroup> GetByNameAsync(string name)
    {
        return await _context.UserGroups
            .FirstOrDefaultAsync(g => g.Name == name);
    }

    public async Task<IEnumerable<UserGroup>> GetAllAsync()
    {
        return await _context.UserGroups.ToListAsync();
    }

    public async Task<IEnumerable<UserGroup>> GetAllActiveAsync()
    {
        return await _context.UserGroups
            .Where(g => g.IsActive)
            .ToListAsync();
    }

    public async Task UpdateAsync(UserGroup userGroup)
    {
        _context.UserGroups.Update(userGroup);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(UserGroup userGroup)
    {
        _context.UserGroups.Remove(userGroup);
        await _context.SaveChangesAsync();
    }
}
