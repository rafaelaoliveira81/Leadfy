namespace Domain.Interface;

public interface ITenantProvider
{
    Guid? TenantId { get; }
    Guid GetRequiredTenantId();
}