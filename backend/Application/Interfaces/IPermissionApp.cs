using Domain.Enuns;

namespace Application;

public interface IPermissionApp
{
    Task<bool> CheckPermissionAsync(int userId, PermissionEnum permission);
}