namespace MimCrm.Api.Infrastructure.Tenancy;

public interface ITenantAccessor
{
    string? ResolveTenantHeader();
}
