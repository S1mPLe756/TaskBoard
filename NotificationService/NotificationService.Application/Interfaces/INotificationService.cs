using NotificationService.Application.DTOs;

namespace NotificationService.Application.Interfaces;

public interface INotificationService
{
    Task<Guid> SendAsync(NotificationRequest request, CancellationToken ct = default);
    Task<Guid> SendAsync(NotificationBulkRequest request, CancellationToken ct = default);

}