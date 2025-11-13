using Organization.Domain.Enums;

namespace Organization.Domain.Entities;

public class WorkspaceMember
{
    public Guid Id { get; set; }
    public Guid WorkspaceId { get; set; }
    
    public Workspace Workspace { get; set; } = null!;
    public Guid UserId { get; set; }
    public WorkspaceRole Role { get; set; }
}
