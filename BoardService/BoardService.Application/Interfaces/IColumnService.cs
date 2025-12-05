using BoardService.Application.DTOs;

namespace BoardService.Application.Interfaces;

public interface IColumnService
{
    Task<BoardColumnResponse> CreateColumnForBoardAsync(CreateColumnBoardRequest request, Guid boardId, Guid userId);
    Task<List<BoardColumnResponse>> GetColumnsBoard(Guid boardId, Guid userId);
}