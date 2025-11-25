using BoardService.Application.DTOs;

namespace BoardService.Application.Interfaces;

public interface IBoardService
{
    Task<BoardResponse> CreateBoardAsync(Guid userId, CreateBoardRequest createBoardRequest);
}