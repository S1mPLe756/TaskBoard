namespace CardService.Domain.Events;

public class BoardDeletionFailedEvent
{
    public Guid BoardId { get; set; }
    public string Message { get; set; }
}