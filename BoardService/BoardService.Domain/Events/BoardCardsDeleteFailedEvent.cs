namespace BoardService.Domain.Events;

public class BoardCardsDeleteFailedEvent
{
    public Guid BoardId { get; init; }
    public string Message { get; init; } = string.Empty;
}