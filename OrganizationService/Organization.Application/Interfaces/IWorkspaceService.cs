using Organization.Application.DTOs;
using Organization.Application.DTOs.Responses;
using Organization.Domain.Enums;

namespace Organization.Application.Interfaces;

public interface IWorkspaceService
{
    Task<WorkspaceResponse> CreateWorkspaceAsync(Guid ownerId, string name);
    Task<WorkspaceResponse> GetWorkspaceAsync(Guid workspaceId);
    Task<List<WorkspaceResponse>> GetUserWorkspacesAsync(Guid userId);
    Task<WorkspaceMemberResponse> AddMemberAsync(Guid workspaceId, Guid userId, WorkspaceRole role);
    Task RemoveMemberAsync(Guid workspaceId, Guid userId);
}