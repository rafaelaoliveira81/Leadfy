using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

/// <summary>
/// Repository responsible for the N:N relationship between UserGroup and Permission.
/// Queries here always include Permission explicitly to avoid lazy-loading during JWT claim generation.
/// Contains only data access logic — no business rules.
/// </summary>
public class UserGroupPermissionRepo : BaseRepo, IUserGroupPermissionRepo
{
    public UserGroupPermissionRepo(CRMContext context) : base(context)
    {
    }

    public async Task<int> AddAsync(UserGroupPermission userGroupPermission)
    {
        _context.UserGroupPermissions.Add(userGroupPermission);
        await _context.SaveChangesAsync();
        return userGroupPermission.ID;
    }

    public async Task<UserGroupPermission> GetByIdAsync(int id)
    {
        return await _context.UserGroupPermissions
            .Include(ugp => ugp.Permission)
            .FirstOrDefaultAsync(ugp => ugp.ID == id);
    }

    /// <summary>
    /// Returns all active entries for a user group with Permission loaded.
    /// Used to build JWT claims during login.
    /// </summary>
    public async Task<IEnumerable<UserGroupPermission>> GetByUserGroupAsync(int userGroupId)
    {
        return await _context.UserGroupPermissions
            .Include(ugp => ugp.Permission)
            .Where(ugp => ugp.UserGroupId == userGroupId && ugp.IsActive)
            .ToListAsync();
    }

    /// <summary>
    /// Returns true if the given group holds the specified permission (active only).
    /// </summary>
    public async Task<bool> CheckPermissionAsync(int userGroupId, int permissionId)
    {
        return await _context.UserGroupPermissions
            .AnyAsync(ugp =>
                ugp.UserGroupId == userGroupId &&
                ugp.PermissionId == permissionId &&
                ugp.IsActive);
    }

    /// <summary>
    /// Removes all entries for the given group. Used when replacing permissions atomically.
    /// </summary>
    public async Task DeleteByUserGroupIdAsync(int userGroupId)
    {
        var entries = await _context.UserGroupPermissions
            .Where(ugp => ugp.UserGroupId == userGroupId)
            .ToListAsync();

        _context.UserGroupPermissions.RemoveRange(entries);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserGroupPermission userGroupPermission)
    {
        _context.UserGroupPermissions.Update(userGroupPermission);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(UserGroupPermission userGroupPermission)
    {
        _context.UserGroupPermissions.Remove(userGroupPermission);
        await _context.SaveChangesAsync();
    }
}
