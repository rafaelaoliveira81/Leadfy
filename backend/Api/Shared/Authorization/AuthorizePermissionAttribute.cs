using Application;
using Api.Shared.Helpers;
using Domain.Enuns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Shared.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class AuthorizePermissionAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly PermissionEnum[] _permissions;

    public AuthorizePermissionAttribute(params PermissionEnum[] permissions)
    {
        _permissions = permissions ?? Array.Empty<PermissionEnum>();
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (_permissions.Length == 0)
            return;

        var user = context.HttpContext.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userId = AuthenticatedUserHelper.GetRequiredUserId(user);
        var permissionApp = context.HttpContext.RequestServices.GetRequiredService<IPermissionApp>();
        var grantedPermissions = AuthenticatedUserHelper.GetPermissions(user);

        foreach (var permission in _permissions)
        {
            if (!grantedPermissions.Contains(permission.ToString(), StringComparer.OrdinalIgnoreCase))
                continue;

            if (await permissionApp.CheckPermissionAsync(userId, permission))
                return;
        }

        context.Result = new ObjectResult(new { message = "Usuário não possui permissão para executar esta operação." })
        {
            StatusCode = StatusCodes.Status403Forbidden
        };
    }
}