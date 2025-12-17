namespace NotificationService.Infrastructure.Messaging.Events;

public class CardUpdatedEvent
{
    public List<string> Emails { get; set; } = null!;
    public string Message { get; set; } = null!;
}