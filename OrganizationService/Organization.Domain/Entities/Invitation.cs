using Organization.Domain.Enums;

namespace Organization.Domain.Entities;

public class Invitation
{
    public Guid Id { get; set; }
    public Guid WorkspaceId { get; set; }
    public string Email { get; set; } = string.Empty;
    public WorkspaceRole Role { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool Accepted { get; set; } = false;

}