using BoardService.Application.DTOs.Requestes;
using BoardService.Application.DTOs.Responses;
using BoardService.Domain.DTOs;
using Refit;

namespace BoardService.Infrastructure.ExternalAPI;

public interface ICardApiRefitClient
{
    [Post("/api/v1/Card/batch")]
    Task<List<CardResponse>> GetCards(GetCardsBatchRequest request);

}