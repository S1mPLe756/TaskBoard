using Organization.Application.DTOs;
using Organization.Application.DTOs.Responses;
using Organization.Domain.Enums;

namespace Organization.Application.Interfaces;

public interface IInvitationService
{
    Task CreateInvitationAsync(Guid workspaceId, string email, WorkspaceRole role);
    Task<AcceptInvtitationResponse> AcceptInvitationAsync(Guid invitationId, Guid userId);
    Task<List<InvitationResponse>> GetPendingInvitationsAsync(string email);

    Task<WorkspaceResponse> GetWorkspaceByInvitationAsync(Guid invitationId);
}