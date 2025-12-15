using CardService.Domain.DTOs;

namespace CardService.Domain.Interfaces;

public interface IBoardApiClient
{
    Task<bool> IsExistBoardWithColumnAsync(Guid boardId, Guid columnId);

    Task<BoardDto?> GetBoardAsync(Guid boardId);

    Task<BoardDto?> GetBoardByCardIdAsync(Guid cardId);
}