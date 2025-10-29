using AuthService.Domain.ValueObjects;

namespace AuthService.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Password Password { get; set; } = null!;
    public string Role { get; set; } = null!;
    public DateTime Created { get; set; } = DateTime.UtcNow;
}