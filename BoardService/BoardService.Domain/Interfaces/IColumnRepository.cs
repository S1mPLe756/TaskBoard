using BoardService.Domain.Entities;

namespace BoardService.Domain.Interfaces;

public interface IColumnRepository
{
    Task<BoardColumn?> GetColumnByIdAsync(Guid id); 
    Task AddColumnAsync(BoardColumn board);
    Task UpdateColumnAsync(BoardColumn board);
    Task<List<BoardColumn>> GetColumnByBoardIdAsync(Guid boardId);
    Task DeleteColumnAsync(BoardColumn board);
}