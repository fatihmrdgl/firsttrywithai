namespace MimCrm.Api.Contracts;

public sealed record RegisterRequest(string TenantName, string TenantSlug, string FullName, string Email, string Password);
public sealed record LoginRequest(string TenantSlug, string Email, string Password);
public sealed record AuthResponse(string Token, Guid UserId, Guid TenantId, string Role);
