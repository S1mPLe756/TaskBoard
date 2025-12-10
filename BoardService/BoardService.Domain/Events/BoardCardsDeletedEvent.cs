namespace BoardService.Domain.Events;

public class BoardCardsDeletedEvent
{
    public Guid BoardId { get; init; }
}