using CardService.Domain.DTOs;
using Refit;

namespace CardService.Infrastructure.ExternalAPI;

public interface IBoardApiRefitClient
{
    [Get("/api/v1/Board/{boardId}/column/{columnId}/exist")]
    Task<bool> IsExistBoardWithColumn(Guid boardId, Guid columnId);
    
    [Get("/api/v1/Board/{boardId}")]
    Task<BoardDto?> GetBoardAsync(Guid boardId);
}