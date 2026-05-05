namespace SubscriptionService.Domain.Events;

public class BoardDeleteEvent
{
    public List<Guid> CardIds { get; set; } = new();
    public Guid BoardId { get; set; }
}