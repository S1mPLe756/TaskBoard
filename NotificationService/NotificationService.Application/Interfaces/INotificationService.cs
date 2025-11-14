using NotificationService.Application.DTOs;

namespace NotificationService.Application.Interfaces;

public interface INotificationService
{
    Task<Guid> SendAsync(NotificationRequest request);
}