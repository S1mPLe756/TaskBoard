using BoardService.Application.DTOs.Requestes;
using BoardService.Application.DTOs.Responses;

namespace BoardService.Application.Interfaces;

public interface ICardService
{
    Task CreateCardPositionAsync(CreatedCardRequest dto);
    Task<ColumnCardResponse> UpdateCardPositionAsync(Guid userId, UpdateCardPositionRequest request);
    Task DeleteCardPositionAsync(Guid messageCardId);
}