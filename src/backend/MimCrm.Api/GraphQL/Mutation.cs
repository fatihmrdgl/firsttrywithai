using HotChocolate.Authorization;
using MimCrm.Api.Contracts;
using MimCrm.Api.Data;
using MimCrm.Api.Domain.Entities;
using MimCrm.Api.Infrastructure.Tenancy;

namespace MimCrm.Api.GraphQL;

public class Mutation
{
    [Authorize(Roles = ["Admin"])]
    public async Task<Product?> CreateProduct(CreateProductRequest request, AppDbContext dbContext, ITenantContext tenantContext, CancellationToken cancellationToken)
    {
        if (tenantContext.TenantId is null)
        {
            return null;
        }

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            TenantId = tenantContext.TenantId.Value
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);
        return product;
    }

    [Authorize(Roles = ["Admin"])]
    public async Task<Customer?> CreateCustomer(CreateCustomerRequest request, AppDbContext dbContext, ITenantContext tenantContext, CancellationToken cancellationToken)
    {
        if (tenantContext.TenantId is null)
        {
            return null;
        }

        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            TenantId = tenantContext.TenantId.Value
        };

        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync(cancellationToken);
        return customer;
    }
}
