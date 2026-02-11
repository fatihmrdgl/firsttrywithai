using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimCrm.Api.Contracts;
using MimCrm.Api.Data;
using MimCrm.Api.Domain.Entities;
using MimCrm.Api.Infrastructure.Tenancy;

namespace MimCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController(AppDbContext dbContext, ITenantContext tenantContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        if (tenantContext.TenantId is null)
        {
            return BadRequest(new { message = "X-Tenant-Id header is required." });
        }

        var items = await dbContext.Customers
            .Where(x => x.TenantId == tenantContext.TenantId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return Ok(items);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        if (tenantContext.TenantId is null)
        {
            return BadRequest(new { message = "X-Tenant-Id header is required." });
        }

        var entity = new Customer
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            TenantId = tenantContext.TenantId.Value
        };

        dbContext.Customers.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Ok(entity);
    }
}
