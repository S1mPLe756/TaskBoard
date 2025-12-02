using Organization.Domain.Enums;

namespace Organization.Application.DTOs.Responses;

public class WorkspaceMemberResponse
{
    public Guid UserId { get; set; }
    public WorkspaceRole Role { get; set; }
}