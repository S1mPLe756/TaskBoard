namespace BoardService.Infrastructure.Messaging.Events;

public class CardCreatedEvent
{
    public Guid CardId { get; set; }
    public  Guid ColumnId { get; set; }
}