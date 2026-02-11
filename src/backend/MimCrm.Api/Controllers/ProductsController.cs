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
public class ProductsController(AppDbContext dbContext, ITenantContext tenantContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        if (tenantContext.TenantId is null)
        {
            return BadRequest(new { message = "X-Tenant-Id header is required." });
        }

        var items = await dbContext.Products
            .Where(x => x.TenantId == tenantContext.TenantId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return Ok(items);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        if (tenantContext.TenantId is null)
        {
            return BadRequest(new { message = "X-Tenant-Id header is required." });
        }

        var entity = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            TenantId = tenantContext.TenantId.Value
        };

        dbContext.Products.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Ok(entity);
    }
}
