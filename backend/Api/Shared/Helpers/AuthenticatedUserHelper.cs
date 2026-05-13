using System.Security.Claims;

namespace Api.Shared.Helpers;

public static class AuthenticatedUserHelper
{
    public static int GetRequiredUserId(ClaimsPrincipal user)
    {
        if (user?.Identity?.IsAuthenticated != true)
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        var claimValue = user.FindFirstValue("usuarioId");
        if (!int.TryParse(claimValue, out var userId) || userId <= 0)
            throw new UnauthorizedAccessException("Claim de usuário inválida.");

        return userId;
    }

    public static string GetEmail(ClaimsPrincipal user)
    {
        return user?.FindFirstValue("email");
    }

    public static string GetName(ClaimsPrincipal user)
    {
        return user?.FindFirstValue("nome");
    }
}