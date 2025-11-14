using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.DbContext;

using Microsoft.EntityFrameworkCore;

public class NotificationDbContext : DbContext
{
    public DbSet<Notification> Notifications { get; set; }

    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options) { }
}