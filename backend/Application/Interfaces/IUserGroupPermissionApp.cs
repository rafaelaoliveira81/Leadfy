using Domain.Entities;
using Domain.Enuns;

namespace Application;

public interface IUserGroupPermissionApp
{
    Task<IReadOnlyCollection<string>> GetPermissionNamesAsync(User user);
    Task<IEnumerable<UserGroupPermission>> GetByUserGroupIdAsync(int userGroupId);
    Task<bool> CheckPermissionAsync(int userGroupId, PermissionEnum permission);
}