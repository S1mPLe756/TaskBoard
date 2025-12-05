using Refit;

namespace BoardService.Domain.Interfaces;

public interface IOrganizationApiRefitClient
{
    [Get("/api/v1/Workspace/{workspaceId}/can-change-workspace/{userId}")]
    Task<bool> CanChangeWorkspaceAsync(Guid workspaceId, Guid userId);
    
    [Get("/api/v1/Workspace/{workspaceId}/can-see-workspace/{userId}")]
    Task<bool> CanSeeWorkspaceAsync(Guid workspaceId, Guid userId);


}