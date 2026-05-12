using Domain.Enuns;

namespace Application;

public class PermissionApp : IPermissionApp
{
    private readonly IUserRepo _userRepo;
    private readonly IUserGroupPermissionApp _userGroupPermissionApp;

    public PermissionApp(IUserRepo userRepo, IUserGroupPermissionApp userGroupPermissionApp)
    {
        _userRepo = userRepo;
        _userGroupPermissionApp = userGroupPermissionApp;
    }

    public async Task<bool> CheckPermissionAsync(int userId, PermissionEnum permission)
    {
        if (userId <= 0)
            return false;

        var user = await _userRepo.GetActiveByIdAsync(userId);
        if (user == null)
            return false;

        if (!user.UserGroupId.HasValue)
            return string.Equals(user.Role.ToString(), permission.ToString(), StringComparison.OrdinalIgnoreCase);

        return await _userGroupPermissionApp.CheckPermissionAsync(user.UserGroupId.Value, permission);
    }
}