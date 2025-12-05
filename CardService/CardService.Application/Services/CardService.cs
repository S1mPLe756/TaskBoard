using System.Net;
using AutoMapper;
using CardService.Application.DTOs;
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
        
        var card = mapper.Map<Card>(request);

        await repository.CreateCardAsync(card);

        await eventPublisher.PublishCardCreatedSendAsync(new()
        {
            ColumnId = request.ColumnId,
            CardId = card.Id
        });
        
        
        return mapper.Map<CardResponse>(card);
    }
}