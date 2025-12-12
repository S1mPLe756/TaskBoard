using CardService.Application.DTOs;
using CardService.Application.DTOs.Requests;
using CardService.Application.DTOs.Responses;

namespace CardService.Application.Interfaces;

public interface ICardService
{
    Task<CardFullResponse> GetCard(Guid cardId);
    Task<CardResponse> CreateCard(CreateCardRequest request, Guid userId);
    Task<List<CardResponse>> GetCardsBatchAsync(GetCardsBatchRequest request);
    Task DeleteCardsAsync(DeleteCardsRequest deleteRequest);
    Task DeleteColumnCardsAsync(DeleteColumnCardsRequest deleteRequest);
    Task<CardFullResponse> UpdateCard(UpdateCardRequest request, Guid userId);
}