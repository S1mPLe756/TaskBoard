using Refit;
using SubscriptionService.Domain.DTOs;

namespace SubscriptionService.Infrastructure.ExternalAPI;

public interface IYookassaApiRefitClient
{
    [Post("/payments")]
    Task<PaymentInfoResponseDto> CreatePaymentAsync(
        [Body] PaymentRequestDto request
    );
    
    [Get("/payments/{id}")]
    Task<Dictionary<String, Object>> GetPaymentAsync(string id);

    [Post("/refunds")]
    Task<Dictionary<String, Object>> CreateRefundAsync([Body] Dictionary<String, Object> request);
}
