
namespace CardService.Domain.Interfaces;

public interface IOrganizationApiClient
{
    Task<bool> CanChangeWorkspaceAsync(Guid workspaceId, Guid userId);
    
    Task<bool> CanSeeWorkspaceAsync(Guid workspaceId, Guid userId);

}