using CardService.Application.DTOs;

namespace CardService.Application.Interfaces;

public interface ICardService
{
    Task<CardResponse> GetCard(Guid cardId);
    Task<CardResponse> CreateCard(CreateCardRequest request, Guid userId);

}