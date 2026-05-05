using SubscriptionService.Domain.Entities;

namespace SubscriptionService.Domain.Interfaces;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetSubscriptionByIdAsync(Guid id);
    Task CreateSubscriptionAsync(Subscription subscription);
    Task UpdateSubscriptionAsync(Subscription subscription);
    Task<List<Subscription>> GetSubscriptionsByIdsAsync(List<Guid> requestSubscriptionIds);
    Task DeleteSubscriptionsAsync(List<Subscription> subscriptions);
    Task DeleteSubscriptionAsync(Subscription subscription);
}