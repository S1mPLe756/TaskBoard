namespace BoardService.Domain.Events;

public class BoardDeleteEvent
{
    public List<Guid> CardIds { get; set; } = new List<Guid>();
    public Guid BoardId { get; set; }
}