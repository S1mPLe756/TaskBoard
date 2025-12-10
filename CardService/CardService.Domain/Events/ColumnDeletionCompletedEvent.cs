namespace CardService.Domain.Events;

public class ColumnDeletionCompletedEvent
{
    public Guid ColumnId { get; set; }
}