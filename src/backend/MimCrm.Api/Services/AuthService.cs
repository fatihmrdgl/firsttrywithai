using Microsoft.EntityFrameworkCore;
using MimCrm.Api.Contracts;
using MimCrm.Api.Data;
using MimCrm.Api.Domain.Entities;
using MimCrm.Api.Security;

namespace MimCrm.Api.Services;

public class AuthService(AppDbContext dbContext, IConfiguration configuration) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var existingTenant = await dbContext.Tenants
            .FirstOrDefaultAsync(x => x.Slug == request.TenantSlug, cancellationToken);

        if (existingTenant is not null)
        {
            throw new InvalidOperationException("Tenant slug is already in use.");
        }

        var tenant = new Tenant
        {
            Name = request.TenantName,
            Slug = request.TenantSlug.Trim().ToLowerInvariant(),
            IsActive = true
        };

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email.Trim().ToLowerInvariant(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.Admin,
            Tenant = tenant
        };

        var defaultPlan = new SubscriptionPlan
        {
            Name = "Starter",
            MonthlyPrice = 29,
            UserLimit = 10,
            StartsAtUtc = DateTime.UtcNow,
            EndsAtUtc = DateTime.UtcNow.AddMonths(1),
            Tenant = tenant
        };

        dbContext.Add(tenant);
        dbContext.Add(user);
        dbContext.Add(defaultPlan);
        await dbContext.SaveChangesAsync(cancellationToken);

        return BuildAuthResponse(user);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var tenantSlug = request.TenantSlug.Trim().ToLowerInvariant();
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await dbContext.Users
            .Include(x => x.Tenant)
            .FirstOrDefaultAsync(x => x.Email == email && x.Tenant.Slug == tenantSlug && x.Tenant.IsActive, cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        return BuildAuthResponse(user);
    }

    private AuthResponse BuildAuthResponse(User user)
    {
        var issuer = configuration["Jwt:Issuer"]!;
        var audience = configuration["Jwt:Audience"]!;
        var key = configuration["Jwt:Key"]!;
        var token = JwtTokenFactory.Create(user, issuer, audience, key);

        return new AuthResponse(token, user.Id, user.TenantId, user.Role.ToString());
    }
}
