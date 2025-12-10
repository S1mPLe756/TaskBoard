namespace BoardService.Domain.Events;

public class ColumnCardsDeleteFailedEvent
{
    public Guid ColumnId { get; set; }
    public string Message { get; set; }
}