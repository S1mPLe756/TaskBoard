namespace CardService.Domain.Events;

public class CardDeletedEvent
{
    public Guid CardId { get; set; }
}