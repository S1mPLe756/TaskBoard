namespace CardService.Domain.Events;

public class BoardDeletionCompletedEvent
{
    public Guid BoardId { get; set; }
}