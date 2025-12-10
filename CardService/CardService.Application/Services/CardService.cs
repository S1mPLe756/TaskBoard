using System.Net;
using AutoMapper;
using CardService.Application.DTOs;
using CardService.Application.DTOs.Requests;
using CardService.Application.DTOs.Responses;
using CardService.Application.Interfaces;
using CardService.Domain.Entities;
using CardService.Domain.Interfaces;
using ExceptionService;

namespace CardService.Application.Services;

public class CardService(
    ICardRepository repository,
    IMapper mapper,
    IBoardApiClient boardApiClient,
    IOrganizationApiClient organizationApiClient,
    ICardLabelRepository labelRepository,
    IEventPublisher eventPublisher)
    : ICardService
{
    public async Task<CardResponse> GetCard(Guid cardId)
    {
        var card = await repository.GetCardByIdAsync(cardId);

        return mapper.Map<CardResponse>(card);
    }

    public async Task<CardResponse> CreateCard(CreateCardRequest request, Guid userId)
    {
        var board = await boardApiClient.GetBoardAsync(request.BoardId);

        if (board == null || !board.Columns.Exists(b => b.Id == request.ColumnId))
        {
            throw new AppException("Board with this column doesn't exist", HttpStatusCode.NotFound);
        }

        if (!await organizationApiClient.CanChangeWorkspaceAsync(board.WorkspaceId, userId))
        {
            throw new AppException("You can't change workspace", HttpStatusCode.Forbidden);
        }

        var existingLabels =
            await labelRepository.GetLabelsByCardIdsAsync(board.Columns.SelectMany(c => c.Cards).Select(x => x.CardId)
                .ToList());
        
        var newLabels = request.Labels
            .Where(l => !existingLabels.Any(el => el.Name.Equals(l.Name, StringComparison.OrdinalIgnoreCase)))
            .ToList();
        
        var card = mapper.Map<Card>(request);
        
        card.Labels = existingLabels
            .Where(el => request.Labels.Any(l => l.Name.Equals(el.Name, StringComparison.OrdinalIgnoreCase)))
            .ToList();
        
        card.Labels.AddRange(newLabels.Select(l => mapper.Map<CardLabel>(l)));


        await repository.CreateCardAsync(card);

        await eventPublisher.PublishCardCreatedSendAsync(new()
        {
            ColumnId = request.ColumnId,
            CardId = card.Id
        });


        return mapper.Map<CardResponse>(card);
    }

    public async Task<List<CardResponse>> GetCardsBatchAsync(GetCardsBatchRequest request)
    {
        if (request.CardIds == null || request.CardIds.Count == 0)
            throw new AppException("CardIds must not be empty");
        var cards = await repository.GetCardsByIdsAsync(request.CardIds);
        return cards.Select(mapper.Map<CardResponse>).ToList();
    }

    public async Task DeleteCardsAsync(DeleteCardsRequest deleteRequest)
    {
        if (deleteRequest.BoardId == Guid.Empty)
            throw new AppException("BoardId must be provided");

        try
        {
            var cards = await repository.GetCardsByIdsAsync(deleteRequest.Cards);

            if (cards.Count > 0)
            {
                await repository.DeleteCardsAsync(cards);
            }

            await eventPublisher.PublishBoardCardsDeletedAsync(new()
            {
                BoardId = deleteRequest.BoardId
            });
        }
        catch (Exception ex)
        {
            await eventPublisher.PublishBoardCardsDeleteFailedAsync(new()
            {
                BoardId = deleteRequest.BoardId,
                Message = ex.Message
            });
            throw;
        }
    }
    
    public async Task DeleteColumnCardsAsync(DeleteColumnCardsRequest deleteRequest)
    {
        if (deleteRequest.ColumnId == Guid.Empty)
            throw new AppException("ColumnId must be provided");

        try
        {
            var cards = await repository.GetCardsByIdsAsync(deleteRequest.Cards);

            if (cards.Count > 0)
            {
                await repository.DeleteCardsAsync(cards);
            }

            await eventPublisher.PublishColumnCardsDeletedAsync(new()
            {
                ColumnId = deleteRequest.ColumnId
            });
        }
        catch (Exception ex)
        {
            await eventPublisher.PublishColumnCardsDeleteFailedAsync(new()
            {
                ColumnId = deleteRequest.ColumnId,
                Message = ex.Message
            });
            throw;
        }
    }
}