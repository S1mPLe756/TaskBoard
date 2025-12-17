using NotificationService.Domain.Entities;

namespace NotificationService.Domain.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(Notification notification);
    Task UpdateAsync(Notification notification);
    Task<Notification?> GetByIdAsync(Guid id);
    Task AddRangeAsync(List<Notification> notifications);
    Task UpdateRangeAsync(List<Notification> notifications);
}