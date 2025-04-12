using Domain.Interfaces;
using SharedKernel;

namespace Domain.Users;

public sealed class User : Entity
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string UserName { get; set; }
    public bool IsChangePassword { get; set; }
    public string? PhoneNumber { get; set; }
    public string PasswordHash { get; set; }
    public string NormalizedUserName { get; set; }
}
