namespace MimCrm.Api.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
}

public enum UserRole
{
    User = 0,
    Admin = 1
}
