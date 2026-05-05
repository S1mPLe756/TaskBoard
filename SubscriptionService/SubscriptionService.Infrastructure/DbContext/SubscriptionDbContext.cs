using SubscriptionService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SubscriptionService.Infrastructure.DbContext;

public class SubscriptionDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public SubscriptionDbContext(DbContextOptions<SubscriptionDbContext> options) : base(options) { }

    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<UserCard> UserCards { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);
    }
}