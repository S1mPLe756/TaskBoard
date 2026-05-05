using System.Net;
using AutoMapper;
using ExceptionService;
using SubscriptionService.Application.DTOs.Responses;
using SubscriptionService.Application.Interfaces;
using SubscriptionService.Domain.Enums;
using SubscriptionService.Domain.Interfaces;

namespace SubscriptionService.Application.Services;

public class SubscriptionService(
    ISubscriptionRepository repository,
    IMapper mapper,
    IEventPublisher eventPublisher) : ISubscriptionService
{
    public async Task<SubscriptionResponse> GetSubscriptionAsync(Guid subscriptionId, Guid userId)
    {
        var subscription = await repository.GetSubscriptionByIdAsync(subscriptionId);

        if (subscription == null)
        {
            throw new AppException("Subscription not found", HttpStatusCode.NotFound);
        }

        return mapper.Map<SubscriptionResponse>(subscription);
    }

    public Task<ConfirmationResponse> CreatePaymentAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<ConfirmationResponse> CreatePaymentWithCardAsync(Guid userId, string paymentMethodId)
    {
        throw new NotImplementedException();
    }

    public Task<ConfirmationResponse> CreatePaymentWithSbpAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<SubscriptionResponse> SetSubscriptionStatusAsync(Guid userId, Guid subscriptionId, SubscriptionStatus status)
    {
        throw new NotImplementedException();
    }

    public Task<SubscriptionResponse> GetSubscriptionAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}