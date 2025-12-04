using BoardService.Application.DTOs;

namespace BoardService.Application.Interfaces;

public interface IBoardService
{
    Task<BoardResponse> CreateBoardAsync(Guid userId, CreateBoardRequest createBoardRequest);
    Task<BoardResponse> GetBoardAsync(Guid userId, Guid boardId);
    Task<List<BoardResponse>> GetBoardByWorkspaceAsync(Guid userId, Guid workspaceId);
}