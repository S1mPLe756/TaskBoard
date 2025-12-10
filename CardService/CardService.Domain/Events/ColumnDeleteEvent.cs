namespace CardService.Domain.Events;

public class ColumnDeleteEvent
{
    public List<Guid> CardIds { get; set; } = new();
    public Guid ColumnId { get; set; }
}