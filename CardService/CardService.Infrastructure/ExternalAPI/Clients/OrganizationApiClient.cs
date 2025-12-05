using CardService.Domain.Interfaces;
using ExceptionService;
using Microsoft.Extensions.Logging;
using Refit;

namespace CardService.Infrastructure.ExternalAPI.Clients;

public class OrganizationApiClient: IOrganizationApiClient
{
    private readonly IOrganizationApiRefitClient _refitClient;
    private readonly ILogger<OrganizationApiClient> _logger;

    public OrganizationApiClient(
        IOrganizationApiRefitClient refitClient, 
        ILogger<OrganizationApiClient> logger)
    {
        _refitClient = refitClient;
        _logger = logger;
    }

    public async Task<bool> CanChangeWorkspaceAsync(Guid workspaceId, Guid userId)
    {
        try
        {
            _logger.LogInformation("Checking board creation permissions for workspace {WorkspaceId}, user {UserId}", 
                workspaceId, userId);
            
            return await _refitClient.CanChangeWorkspaceAsync(workspaceId, userId);
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, "Error calling Organization API for workspace {WorkspaceId}, user {UserId}", 
                workspaceId, userId);
            throw new AppException("Organization service unavailable");
        }
    }
    
    public async Task<bool> CanSeeWorkspaceAsync(Guid workspaceId, Guid userId)
    {
        try
        {
            _logger.LogInformation("Checking see permissions for workspace {WorkspaceId}, user {UserId}", 
                workspaceId, userId);
            
            return await _refitClient.CanSeeWorkspaceAsync(workspaceId, userId);
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, "Error calling Organization API for workspace {WorkspaceId}, user {UserId}", 
                workspaceId, userId);
            throw new AppException("Organization service unavailable");
        }
    }
}