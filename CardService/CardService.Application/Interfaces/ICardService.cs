using CardService.Application.DTOs;
using CardService.Application.DTOs.Requests;
using CardService.Application.DTOs.Responses;
using Microsoft.AspNetCore.Http;

namespace CardService.Application.Interfaces;

public interface ICardService
{
    Task<CardFullResponse> GetCard(Guid cardId);
    Task<CardResponse> CreateCard(CreateCardRequest request, Guid userId);
    Task<List<CardResponse>> GetCardsBatchAsync(GetCardsBatchRequest request);
    Task DeleteCardsAsync(DeleteCardsRequest deleteRequest);
    Task DeleteColumnCardsAsync(DeleteColumnCardsRequest deleteRequest);
    Task<CardFullResponse> UpdateCard(UpdateCardRequest request, Guid userId);
    Task<CardFullResponse> AddAttachmentAsync(Guid cardId, IFormFile file, Guid userId);
}