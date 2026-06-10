using System.Security.Claims;

namespace Api.Helpers;

public static class UserClaimsHelper
{
    private const string UserIdClaimType = "usuarioId";

    public static string GetAuthenticatedUserId(this ClaimsPrincipal user)
    {
        if (user?.Identity?.IsAuthenticated != true)
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        var userIdClaim = user.FindFirst(UserIdClaimType)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
            throw new UnauthorizedAccessException($"Claim '{UserIdClaimType}' não encontrada na requisição.");

        if (!Guid.TryParse(userIdClaim, out _))
            throw new UnauthorizedAccessException($"Claim '{UserIdClaimType}' inválida na requisição.");

        return userIdClaim;
    }
}