using SubscriptionService.Application.DTOs.Responses;
using SubscriptionService.Domain.Enums;

namespace SubscriptionService.Application.Interfaces;

public interface ISubscriptionService
{
    Task<SubscriptionResponse> GetSubscriptionAsync(Guid subscriptionId, Guid userId);
    
    Task<ConfirmationResponse> CreatePaymentAsync(Guid userId);
    
    Task<ConfirmationResponse> CreatePaymentWithCardAsync(Guid userId, string paymentMethodId);
    
    Task<ConfirmationResponse> CreatePaymentWithSbpAsync(Guid userId);
    
    Task<SubscriptionResponse> SetSubscriptionStatusAsync(Guid userId, Guid subscriptionId, SubscriptionStatus status);
    
    Task<SubscriptionResponse> GetSubscriptionAsync(Guid userId);
}