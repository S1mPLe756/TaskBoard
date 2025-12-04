using Refit;

namespace BoardService.Domain.Interfaces;

public interface IOrganizationApiRefitClient
{
    [Get("/api/v1/Workspace/{workspaceId}/can-change-workspace/{userId}")]
    Task<bool> CanCreateBoardAsync(Guid workspaceId, Guid userId);

}