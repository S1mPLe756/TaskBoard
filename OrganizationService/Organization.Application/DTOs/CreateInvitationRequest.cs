using Organization.Domain.Enums;

namespace Organization.Application.DTOs;

public class CreateInvitationRequest
{
    public Guid WorkspaceId { get; set; }
    public string Email { get; set; }
    public WorkspaceRole Role { get; set; }
}