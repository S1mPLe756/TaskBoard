using CardService.Domain.Events;

namespace CardService.Domain.Interfaces;

public interface IEventPublisher
{
    Task PublishCardCreatedSendAsync(CardCreatedEvent evt);
    Task PublishBoardCardsDeletedAsync(BoardDeletionCompletedEvent evt);
    Task PublishBoardCardsDeleteFailedAsync(BoardDeletionFailedEvent evt);
    
    Task PublishColumnCardsDeletedAsync(ColumnDeletionCompletedEvent evt);
    Task PublishColumnCardsDeleteFailedAsync(ColumnDeletionFailedEvent evt);
    Task PublishFilesDeletedAsync(FilesDeletedEvent evt);
    Task PublishCardDeletedAsync(CardDeletedEvent evt);
}