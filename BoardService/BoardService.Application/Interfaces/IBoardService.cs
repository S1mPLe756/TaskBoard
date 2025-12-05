using BoardService.Application.DTOs;

namespace BoardService.Application.Interfaces;

public interface IBoardService
{
    Task<BoardResponse> CreateBoardAsync(Guid userId, CreateBoardRequest createBoardRequest);
    Task<BoardResponse> GetBoardAsync(Guid userId, Guid boardId);
    Task<List<BoardResponse>> GetBoardsByWorkspaceAsync(Guid userId, Guid workspaceId);
    Task<bool> IsExistBoardWithColumnAsync(Guid boardId, Guid columnId);
}