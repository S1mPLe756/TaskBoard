namespace BoardService.Domain.Events;

public class ColumnDeleteEvent
{
    public List<Guid> CardIds { get; set; } = new List<Guid>();
    public Guid ColumnId { get; set; }

}