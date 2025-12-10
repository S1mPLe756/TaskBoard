using BoardService.Application.DTOs;
using BoardService.Application.DTOs.Requestes;
using BoardService.Application.DTOs.Responses;

namespace BoardService.Application.Interfaces;

public interface IBoardService
{
    Task<BoardResponse> CreateBoardAsync(Guid userId, CreateBoardRequest createBoardRequest);
    Task<BoardResponse> GetBoardAsync(Guid userId, Guid boardId);
    Task<BoardsResponse> GetBoardsByWorkspaceAsync(Guid userId, Guid workspaceId);
    Task<bool> IsExistBoardWithColumnAsync(Guid boardId, Guid columnId);
    Task<BoardFullResponse> GetBoardFullAsync(Guid userId, Guid boardId);
    Task DeleteBoardAsync(Guid userId, Guid boardId);
    Task CompleteBoardDeletionAsync(Guid evtBoardId);
    Task FailedBoardDeleteAsync(Guid evtBoardId);
    Task UpdateBoardAsync(Guid userId, Guid boardId, UpdateBoardRequest request);
}