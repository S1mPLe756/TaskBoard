using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces;
using NotificationService.Infrastructure.DbContext;

namespace NotificationService.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly NotificationDbContext _db;

    public NotificationRepository(NotificationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Notification notification)
    {
        _db.Notifications.Add(notification);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Notification notification)
    {
        _db.Notifications.Update(notification);
        await _db.SaveChangesAsync();
    }

    public Task<Notification?> GetByIdAsync(Guid id)
    {
        return _db.Notifications.FirstOrDefaultAsync(x => x.Id == id);
    }

}