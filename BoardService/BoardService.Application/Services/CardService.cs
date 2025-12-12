using System.Net;
using AutoMapper;
using BoardService.Application.DTOs.Requestes;
using BoardService.Application.DTOs.Responses;
using BoardService.Application.Interfaces;
using BoardService.Domain.Entities;
using BoardService.Domain.Interfaces;
using ExceptionService;

namespace BoardService.Application.Services;

public class CardService(ICardRepository repository, IColumnRepository columnRepository, IMapper mapper, IOrganizationApiClient organizationApiClient) : ICardService
{
    public async Task CreateCardPositionAsync(CreatedCardRequest dto)
    {
        var card = mapper.Map<CardPosition>(dto);

        card.Position = await repository.CountAsync(dto.ColumnId);
        
        await repository.AddCardPositionAsync(card);
    }

    public async Task<ColumnCardResponse> UpdateCardPositionAsync(Guid userId, UpdateCardPositionRequest request)
    {
        var card = await repository.GetCardPositionByIdAsync(request.CardId);
        
        if (card == null)
        {
            throw new AppException("Card not found", HttpStatusCode.NotFound);
        }

        if (!await organizationApiClient.CanChangeWorkspaceAsync(card.Column.Board.WorkspaceId, userId))
        {
            throw new AppException("Can't change position for this card", HttpStatusCode.Forbidden);
        }
        var sourceColumn = await columnRepository.GetColumnByIdAsync(card.ColumnId);
        var targetColumn = await columnRepository.GetColumnByIdAsync(request.ColumnId);

        if (targetColumn == null)
            throw new AppException("Target column not found", HttpStatusCode.NotFound);

        // Убираем карточку из исходной колонки
        var sourceCards = sourceColumn.Cards.OrderBy(c => c.Position).ToList();
        sourceCards.Remove(card);

        // Вставляем в новую колонку
        var targetCards = targetColumn.Cards.OrderBy(c => c.Position).ToList();
        targetCards.Insert(request.Position, card);

        card.ColumnId = targetColumn.Id;

        for (int i = 0; i < targetCards.Count; i++)
            targetCards[i].Position = i;

        for (int i = 0; i < sourceCards.Count; i++)
            sourceCards[i].Position = i;

        await repository.SaveChangesAsync();

        return mapper.Map<ColumnCardResponse>(card);
    }
}