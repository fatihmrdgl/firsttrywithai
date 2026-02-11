using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;
using MimCrm.Api.Data;
using MimCrm.Api.Domain.Entities;
using MimCrm.Api.Infrastructure.Tenancy;

namespace MimCrm.Api.GraphQL;

public class Query
{
    [Authorize]
    public async Task<IEnumerable<Product>> GetProducts(AppDbContext dbContext, ITenantContext tenantContext, CancellationToken cancellationToken)
    {
        if (tenantContext.TenantId is null)
        {
            return [];
        }

        return await dbContext.Products
            .Where(x => x.TenantId == tenantContext.TenantId)
            .ToListAsync(cancellationToken);
    }

    [Authorize]
    public async Task<IEnumerable<Customer>> GetCustomers(AppDbContext dbContext, ITenantContext tenantContext, CancellationToken cancellationToken)
    {
        if (tenantContext.TenantId is null)
        {
            return [];
        }

        return await dbContext.Customers
            .Where(x => x.TenantId == tenantContext.TenantId)
            .ToListAsync(cancellationToken);
    }
}
