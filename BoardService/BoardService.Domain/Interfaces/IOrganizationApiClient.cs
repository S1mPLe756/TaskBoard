
namespace BoardService.Domain.Interfaces;

public interface IOrganizationApiClient
{
    Task<bool> CanCreateBoardAsync(Guid workspaceId, Guid userId);
}