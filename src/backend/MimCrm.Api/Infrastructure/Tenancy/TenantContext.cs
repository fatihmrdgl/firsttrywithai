namespace MimCrm.Api.Infrastructure.Tenancy;

public class TenantContext : ITenantContext
{
    public Guid? TenantId { get; set; }
    public string? TenantSlug { get; set; }
}
