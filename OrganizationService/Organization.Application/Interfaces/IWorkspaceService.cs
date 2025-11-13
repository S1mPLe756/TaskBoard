using Organization.Application.DTOs;
using Organization.Domain.Enums;

namespace Organization.Application.Interfaces;

public interface IWorkspaceService
{
    Task<WorkspaceDto> CreateWorkspaceAsync(Guid ownerId, string name);
    Task<WorkspaceDto> GetWorkspaceAsync(Guid workspaceId);
    Task<List<WorkspaceDto>> GetUserWorkspacesAsync(Guid userId);
    Task<WorkspaceMemberDto> AddMemberAsync(Guid workspaceId, Guid userId, WorkspaceRole role);
    Task RemoveMemberAsync(Guid workspaceId, Guid userId);
}