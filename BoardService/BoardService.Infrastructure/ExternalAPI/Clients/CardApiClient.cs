using BoardService.Application.DTOs.Requestes;
using BoardService.Domain.DTOs;
using BoardService.Domain.Interfaces;
using ExceptionService;
using Microsoft.Extensions.Logging;
using Refit;

namespace BoardService.Infrastructure.ExternalAPI.Clients;

public class CardApiClient : ICardApiClient
{
    private readonly ICardApiRefitClient _refitClient;
    private readonly ILogger<OrganizationApiClient> _logger;

    public CardApiClient(
        ICardApiRefitClient refitClient, 
        ILogger<OrganizationApiClient> logger)
    {
        _refitClient = refitClient;
        _logger = logger;
    }

    public async Task<List<CardResponse>> GetCards(GetCardsBatchRequest cardsBatchRequest)
    {
        try
        {
            _logger.LogInformation("Get cards batch request");
            
            return await _refitClient.GetCards(cardsBatchRequest);
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, "Error calling Card API for get cards batch request");
            throw new AppException("Card service unavailable");
        }
    }
}