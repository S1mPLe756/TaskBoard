using BoardService.Application.DTOs;
using BoardService.Application.DTOs.Requestes;
using BoardService.Application.DTOs.Responses;

namespace BoardService.Application.Interfaces;

public interface IColumnService
{
    Task<BoardColumnResponse> CreateColumnForBoardAsync(CreateColumnBoardRequest request, Guid boardId, Guid userId);
    Task<List<BoardColumnResponse>> GetColumnsBoard(Guid boardId, Guid userId);
    Task DeleteColumnAsync(Guid columnId, Guid userId);
    Task CompleteColumnDeletionAsync(Guid evtColumnId);
    Task FailedColumnDeleteAsync(Guid evtColumnId);
    Task<BoardColumnResponse> UpdateColumnAsync(Guid columnId, Guid userId, UpdateColumnBoardRequest request);
}