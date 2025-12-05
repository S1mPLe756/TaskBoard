using CardService.Domain.Events;

namespace CardService.Domain.Interfaces;

public interface IEventPublisher
{
    Task PublishCardCreatedSendAsync(CardCreatedEvent evt);
}