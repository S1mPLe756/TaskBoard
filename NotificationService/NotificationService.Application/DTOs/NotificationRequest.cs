using NotificationService.Domain.Enums;

namespace NotificationService.Application.DTOs;

public class NotificationRequest
{
    public string To { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
}
