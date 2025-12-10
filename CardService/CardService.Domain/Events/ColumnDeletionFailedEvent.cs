namespace CardService.Domain.Events;

public class ColumnDeletionFailedEvent
{
    public Guid ColumnId { get; set; }
    public string Message { get; set; }
}