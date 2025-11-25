using BoardService.Domain.Entities;

namespace BoardService.Domain.Interfaces;

public interface IBoardRepository
{
    Task<Board?> GetBoardByIdAsync(Guid id); 
    Task AddBoardAsync(Board board);
    Task UpdateBoardAsync(Board board);
}