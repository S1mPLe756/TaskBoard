using NotificationService.Domain.Enums;

namespace NotificationService.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string To { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Sent { get; set; }
    public string? Error { get; set; }
}
