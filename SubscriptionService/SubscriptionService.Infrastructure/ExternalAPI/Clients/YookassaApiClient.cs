using ExceptionService;
using Microsoft.Extensions.Logging;
using Refit;
using SubscriptionService.Domain.DTOs;

namespace SubscriptionService.Infrastructure.ExternalAPI.Clients;

public class YookassaApiClient : IYookassaApiRefitClient
{
    private readonly IYookassaApiRefitClient _refitClient;
    private readonly ILogger<YookassaApiClient> _logger;

    public YookassaApiClient(
        IYookassaApiRefitClient refitClient, 
        ILogger<YookassaApiClient> logger)
    {
        _refitClient = refitClient;
        _logger = logger;
    }
    
    public async Task<PaymentInfoResponseDto> CreatePaymentAsync(PaymentRequestDto request)
    {
        try
        {
            _logger.LogInformation("Create Payment for {amount}", 
                request.Amount);
            
            return await _refitClient.CreatePaymentAsync(request);
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, "Error calling Create Payment");
            throw new AppException("Yookassa unavailable");
        }
    }

    public async Task<Dictionary<string, object>> GetPaymentAsync(string id)
    {
        try
        {
            _logger.LogInformation($"Get Payment for {id}");
            
            return await _refitClient.GetPaymentAsync(id);
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, "Error calling get Payment");
            throw new AppException("Yookassa unavailable");
        }
    }

    public async Task<Dictionary<string, object>> CreateRefundAsync(Dictionary<string, object> request)
    {
        try
        {
            _logger.LogInformation("Create Refund");
            
            return await _refitClient.CreateRefundAsync(request);
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, "Error calling Create Refund");
            throw new AppException("Yookassa unavailable");
        }
    }
}