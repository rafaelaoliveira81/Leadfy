using Domain.Entities;
using Domain.Enuns;

namespace Application;

public class UserGroupPermissionApp : IUserGroupPermissionApp
{
    private readonly IUserGroupPermissionRepo _userGroupPermissionRepo;

    public UserGroupPermissionApp(IUserGroupPermissionRepo userGroupPermissionRepo)
    {
        _userGroupPermissionRepo = userGroupPermissionRepo;
    }

    public async Task<IReadOnlyCollection<string>> GetPermissionNamesAsync(User user)
    {
        if (user == null)
            throw new ArgumentException("Usuário não pode ser vazio.");

        if (!user.UserGroupId.HasValue)
            return new[] { user.Role.ToString() };

        var permissions = user.UserGroup?.UserGroupPermissions;

        if (permissions == null || permissions.Count == 0)
        {
            permissions = (await _userGroupPermissionRepo.GetByUserGroupAsync(user.UserGroupId.Value)).ToList();
        }

        return permissions
            .Where(permission => permission.IsActive && permission.Permission != null && permission.Permission.IsActive)
            .Select(permission => permission.Permission.Name)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public async Task<IEnumerable<UserGroupPermission>> GetByUserGroupIdAsync(int userGroupId)
    {
        if (userGroupId <= 0)
            throw new ArgumentException("Grupo de usuário inválido.");

        return await _userGroupPermissionRepo.GetByUserGroupAsync(userGroupId);
    }

    public async Task<bool> CheckPermissionAsync(int userGroupId, PermissionEnum permission)
    {
        if (userGroupId <= 0)
            return false;

        return await _userGroupPermissionRepo.CheckPermissionAsync(userGroupId, (int)permission);
    }
}