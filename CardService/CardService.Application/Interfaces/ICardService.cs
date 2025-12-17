using CardService.Application.DTOs;
using CardService.Application.DTOs.Requests;
using CardService.Application.DTOs.Responses;
using Microsoft.AspNetCore.Http;

namespace CardService.Application.Interfaces;

public interface ICardService
{
    Task<CardFullResponse> GetCardAsync(Guid cardId, Guid userId);
    Task<CardResponse> CreateCardAsync(CreateCardRequest request, Guid userId);
    Task<List<CardResponse>> GetCardsBatchAsync(GetCardsBatchRequest request);
    Task DeleteCardsAsync(DeleteCardsRequest deleteRequest);
    Task DeleteColumnCardsAsync(DeleteColumnCardsRequest deleteRequest);
    Task<CardFullResponse> UpdateCardAsync(UpdateCardRequest request, Guid userId);

    Task DeleteCardAsync(Guid id, Guid userId);
    Task WatchCardAsync(Guid cardId, Guid userId);
    Task UnWatchCardAsync(Guid cardId, Guid userId);
}