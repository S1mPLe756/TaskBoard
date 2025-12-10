using BoardService.Domain.Events;

namespace BoardService.Domain.Interfaces;

public interface IEventPublisher
{
    Task PublishBoardDeleteSendAsync(BoardDeleteEvent evt);
    Task PublishColumnDeleteSendAsync(ColumnDeleteEvent evt);

}