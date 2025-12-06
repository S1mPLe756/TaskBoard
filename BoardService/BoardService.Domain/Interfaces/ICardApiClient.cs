using BoardService.Domain.DTOs;

namespace BoardService.Domain.Interfaces;

public interface ICardApiClient
{
    Task<List<CardResponse>> GetCards(GetCardsBatchRequest cardsBatchRequest);
}