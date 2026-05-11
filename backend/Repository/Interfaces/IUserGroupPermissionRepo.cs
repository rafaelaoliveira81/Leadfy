using Domain.Entities;

public interface IUserGroupPermissionRepo
{
    Task<int> AddAsync(UserGroupPermission userGroupPermission);
    Task<UserGroupPermission> GetByIdAsync(int id);

    /// <summary>
    /// Returns all active UserGroupPermission entries for a given group,
    /// with Permission included. Used to build JWT claims.
    /// </summary>
    Task<IEnumerable<UserGroupPermission>> GetByUserGroupAsync(int userGroupId);

    /// <summary>
    /// Checks whether a group holds a specific permission by its ID.
    /// </summary>
    Task<bool> CheckPermissionAsync(int userGroupId, int permissionId);

    /// <summary>
    /// Removes all UserGroupPermission entries for a given group.
    /// Used when replacing a group's permissions atomically.
    /// </summary>
    Task DeleteByUserGroupIdAsync(int userGroupId);

    Task UpdateAsync(UserGroupPermission userGroupPermission);
    Task DeleteAsync(UserGroupPermission userGroupPermission);
}
