namespace MimCrm.Api.Infrastructure.Tenancy;

public class HttpHeaderTenantAccessor(IHttpContextAccessor httpContextAccessor) : ITenantAccessor
{
    public string? ResolveTenantHeader()
    {
        var headers = httpContextAccessor.HttpContext?.Request.Headers;
        if (headers is null)
        {
            return null;
        }

        return headers.TryGetValue("X-Tenant-Id", out var tenantId) ? tenantId.ToString() : null;
    }
}
