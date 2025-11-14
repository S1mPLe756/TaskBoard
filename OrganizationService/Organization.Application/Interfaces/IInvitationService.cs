using Organization.Application.DTOs;
using Organization.Domain.Enums;

namespace Organization.Application.Interfaces;

public interface IInvitationService
{
    Task<InvitationResponse> CreateInvitationAsync(Guid workspaceId, string email, WorkspaceRole role);
    Task AcceptInvitationAsync(Guid invitationId, Guid userId);
    Task<List<InvitationResponse>> GetPendingInvitationsAsync(string email);

}