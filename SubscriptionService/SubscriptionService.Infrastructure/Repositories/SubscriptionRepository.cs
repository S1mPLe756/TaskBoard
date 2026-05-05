using System.Data.Entity;
using SubscriptionService.Domain.Entities;
using SubscriptionService.Domain.Interfaces;
using SubscriptionService.Infrastructure.DbContext;

namespace SubscriptionService.Infrastructure.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private SubscriptionDbContext _context;

    public SubscriptionRepository(SubscriptionDbContext context)
    {
        _context = context;
    }
    
    public async Task<Subscription?> GetSubscriptionByIdAsync(Guid id)
    {
        return await _context.Subscriptions.FirstOrDefaultAsync(c => c.Id == id);;
    }

    public async Task CreateSubscriptionAsync(Subscription subscription)
    {
        await _context.Subscriptions.AddAsync(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSubscriptionAsync(Subscription subscription)
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<Subscription>> GetSubscriptionsByIdsAsync(List<Guid> requestSubscriptionIds)
    {
        return await _context.Subscriptions.Where(c => requestSubscriptionIds.Contains(c.Id))
            .ToListAsync();
    }

    public async Task DeleteSubscriptionsAsync(List<Subscription> subscriptions)
    {
        _context.Subscriptions.RemoveRange(subscriptions);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSubscriptionAsync(Subscription subscription)
    {
        _context.Subscriptions.Remove(subscription);
        await _context.SaveChangesAsync();
    }
}