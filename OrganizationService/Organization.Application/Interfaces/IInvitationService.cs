using Organization.Application.DTOs;
using Organization.Domain.Enums;

namespace Organization.Application.Interfaces;

public interface IInvitationService
{
    Task<InvitationDto> CreateInvitationAsync(Guid workspaceId, string email, WorkspaceRole role);
    Task AcceptInvitationAsync(Guid invitationId, Guid userId);
    Task<List<InvitationDto>> GetPendingInvitationsAsync(string email);

}