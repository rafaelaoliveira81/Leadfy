using Domain.Entities;
using Domain.Enuns;
using Repository.Context;

namespace Application;

public class UserGroupPermissionApp : IUserGroupPermissionApp
{
    private readonly IUserGroupPermissionRepo _userGroupPermissionRepo;
    private readonly IUserGroupRepo _userGroupRepo;
    private readonly IPermissionRepo _permissionRepo;
    private readonly CRMContext _context;

    public UserGroupPermissionApp(
        IUserGroupPermissionRepo userGroupPermissionRepo,
        IUserGroupRepo userGroupRepo,
        IPermissionRepo permissionRepo,
        CRMContext context)
    {
        _userGroupPermissionRepo = userGroupPermissionRepo;
        _userGroupRepo = userGroupRepo;
        _permissionRepo = permissionRepo;
        _context = context;
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

    public async Task UpdateGroupPermissionsAsync(int userGroupId, IReadOnlyCollection<int> permissionIds)
    {
        if (userGroupId <= 0)
            throw new ArgumentException("Grupo de usuário inválido.");

        var userGroup = await _userGroupRepo.GetByIdAsync(userGroupId);
        if (userGroup == null || !userGroup.IsActive)
            throw new KeyNotFoundException("Grupo de usuário não localizado.");

        var distinctPermissionIds = (permissionIds ?? Array.Empty<int>())
            .Where(permissionId => permissionId > 0)
            .Distinct()
            .ToList();

        foreach (var permissionId in distinctPermissionIds)
        {
            var permission = await _permissionRepo.GetByIdAsync(permissionId);
            if (permission == null || !permission.IsActive)
                throw new KeyNotFoundException($"Permissão {permissionId} não localizada.");
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _userGroupPermissionRepo.DeleteByUserGroupIdAsync(userGroupId);

            foreach (var permissionId in distinctPermissionIds)
            {
                await _userGroupPermissionRepo.AddAsync(new UserGroupPermission
                {
                    UserGroupId = userGroupId,
                    PermissionId = permissionId
                });
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}