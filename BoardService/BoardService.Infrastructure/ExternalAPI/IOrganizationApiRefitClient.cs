using Refit;

namespace BoardService.Domain.Interfaces;

public interface IOrganizationApiRefitClient
{
    [Post("/api/v1/Organizations/{workspaceId}/can-create-board")]
    Task<bool> CanCreateBoardAsync(Guid workspaceId, [Body] Guid userId);

}