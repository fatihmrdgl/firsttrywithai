using Microsoft.EntityFrameworkCore;
using MimCrm.Api.Data;

namespace MimCrm.Api.Infrastructure.Tenancy;

public class TenantResolutionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, AppDbContext dbContext, ITenantAccessor tenantAccessor, ITenantContext tenantContext)
    {
        var tenantHeader = tenantAccessor.ResolveTenantHeader();

        if (!string.IsNullOrWhiteSpace(tenantHeader))
        {
            if (Guid.TryParse(tenantHeader, out var tenantId))
            {
                var tenant = await dbContext.Tenants.FirstOrDefaultAsync(x => x.Id == tenantId && x.IsActive);
                if (tenant is not null)
                {
                    tenantContext.TenantId = tenant.Id;
                    tenantContext.TenantSlug = tenant.Slug;
                }
            }
            else
            {
                var tenant = await dbContext.Tenants.FirstOrDefaultAsync(x => x.Slug == tenantHeader && x.IsActive);
                if (tenant is not null)
                {
                    tenantContext.TenantId = tenant.Id;
                    tenantContext.TenantSlug = tenant.Slug;
                }
            }
        }

        await next(context);
    }
}
