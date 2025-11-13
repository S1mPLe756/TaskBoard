using Organization.Domain.Enums;

namespace Organization.Application.DTOs;

public class WorkspaceMemberDto
{
    public Guid UserId { get; set; }
    public WorkspaceRole Role { get; set; }
}