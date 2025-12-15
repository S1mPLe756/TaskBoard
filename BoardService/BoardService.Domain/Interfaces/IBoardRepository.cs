using BoardService.Domain.Entities;

namespace BoardService.Domain.Interfaces;

public interface IBoardRepository
{
    Task<Board?> GetBoardByIdAsync(Guid id); 
    Task<Board?> GetBoardByCardIdAsync(Guid cardId);
    Task AddBoardAsync(Board board);
    Task UpdateBoardAsync(Board board);
    Task<List<Board>> GetBoardsByWorkspaceAsync(Guid workspaceId);
    Task DeleteBoardAsync(Board board);
}