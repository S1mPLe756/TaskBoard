using System.Net;
using ExceptionService;
using Microsoft.Extensions.Logging;
using Organization.Domain.DTOs;
using Organization.Domain.Interfaces;
using Refit;

namespace Organization.Infrastructure.ExternalAPI.Clients;

public class UserApiClient(IUserApiRefitClient refitClient, ILogger<UserApiClient> logger) : IUserApiClient
{
    public async Task<List<UserResponse>> GetUsers(List<Guid> ids)
    {
        try
        {
            logger.LogInformation("Get users batch request");
            
            return await refitClient.GetUsers(ids);
        }
        catch (ApiException ex)
        {
            logger.LogError(ex, "Error calling Auth API for get users batch request");
            throw new AppException("Auth service unavailable");
        }
    }

    public async Task<UserResponse?> GetUserByEmail(string email)
    {
        try
        {
            logger.LogInformation("Get user by email request");
            
            return await refitClient.GetUserByEmail(email);
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.NoContent)
        {
            return null;
        }
        catch (ApiException ex)
        {
            logger.LogError(ex, "Error calling Auth API for get user by email request");
            throw new AppException("Auth service unavailable");
        }
    }
}