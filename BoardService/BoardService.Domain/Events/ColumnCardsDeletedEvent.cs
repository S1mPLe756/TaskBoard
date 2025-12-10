namespace BoardService.Domain.Events;

public class ColumnCardsDeletedEvent
{
    public Guid ColumnId { get; set; }
}