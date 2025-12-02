using Organization.Domain.Enums;

namespace Organization.Application.DTOs.Responses;

public class InvitationResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public WorkspaceRole Role { get; set; }
    public DateTime ExpiresAt { get; set; }
}