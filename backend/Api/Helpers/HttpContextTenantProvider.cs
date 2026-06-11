using System.Security.Claims;
using Domain.Interface;

namespace Api.Helpers;

public class HttpContextTenantProvider : ITenantProvider
{
    private const string TenantIdClaimType = "tenantId";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? TenantId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated != true)
                return null;

            var tenantIdClaim = user.FindFirst(TenantIdClaimType)?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim))
                throw new UnauthorizedAccessException($"Claim '{TenantIdClaimType}' não encontrada na requisição.");

            if (!Guid.TryParse(tenantIdClaim, out var tenantId))
                throw new UnauthorizedAccessException($"Claim '{TenantIdClaimType}' inválida na requisição.");

            return tenantId;
        }
    }

    public Guid GetRequiredTenantId()
    {
        return TenantId ?? throw new UnauthorizedAccessException("Tenant do usuário autenticado não encontrado.");
    }
}