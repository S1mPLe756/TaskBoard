using System.Net;
using AutoMapper;
using CardService.Application.DTOs;
using CardService.Application.DTOs.Requests;
using CardService.Application.DTOs.Responses;
using CardService.Application.Interfaces;
using CardService.Domain.DTOs;
using CardService.Domain.Entities;
using CardService.Domain.Interfaces;
using ExceptionService;
using Microsoft.AspNetCore.Http;

namespace CardService.Application.Services;

public class CardService(
    ICardRepository repository,
    IMapper mapper,
    IBoardApiClient boardApiClient,
    IOrganizationApiClient organizationApiClient,
    ICardLabelRepository labelRepository,
    IUserApiClient userApiClient,
    IEventPublisher eventPublisher)
    : ICardService
{
    public async Task<CardFullResponse> GetCardAsync(Guid cardId, Guid userId)
    {
        var card = await repository.GetCardByIdAsync(cardId);

        if (card == null)
        {
            throw new AppException("Card not found", HttpStatusCode.NotFound);
        }

        await GetBoardAndCheckAsync(
            cardId,
            userId,
            organizationApiClient.CanSeeWorkspaceAsync,
            boardApiClient.GetBoardByCardIdAsync
        );

        var cardResponse = mapper.Map<CardFullResponse>(card);

        cardResponse.IsWatched = card.WatcherIds.Contains(userId);

        await FillUsersAsync(card, cardResponse);

        return cardResponse;
    }

    public async Task<CardResponse> CreateCardAsync(CreateCardRequest request, Guid userId)
    {
        var board = await GetBoardAndCheckAsync(
            request.BoardId,
            userId,
            organizationApiClient.CanChangeWorkspaceAsync,
            boardApiClient.GetBoardAsync
        );


        var existingLabels =
            await labelRepository.GetLabelsByCardIdsAsync(board.Columns.SelectMany(c => c.Cards).Select(x => x.CardId)
                .ToList());

        var newLabels = request.Labels
            .Where(l => !existingLabels.Any(el => el.Name.Equals(l.Name, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        var card = mapper.Map<Card>(request);

        card.DueDate = request.DueDate?.ToUniversalTime() ?? card.DueDate;


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

        await DeleteCardsInternalAsync(
            deleteRequest.Cards,
            () => eventPublisher.PublishBoardCardsDeletedAsync(new() { BoardId = deleteRequest.BoardId }),
            ex => eventPublisher.PublishBoardCardsDeleteFailedAsync(new()
                { BoardId = deleteRequest.BoardId, Message = ex.Message })
        );
    }

    public async Task DeleteColumnCardsAsync(DeleteColumnCardsRequest deleteRequest)
    {
        if (deleteRequest.ColumnId == Guid.Empty)
            throw new AppException("ColumnId must be provided");

        await DeleteCardsInternalAsync(
            deleteRequest.Cards,
            () => eventPublisher.PublishColumnCardsDeletedAsync(new() { ColumnId = deleteRequest.ColumnId }),
            ex => eventPublisher.PublishColumnCardsDeleteFailedAsync(new()
                { ColumnId = deleteRequest.ColumnId, Message = ex.Message })
        );
    }

    public async Task<CardFullResponse> UpdateCardAsync(UpdateCardRequest request, Guid userId)
    {
        var card = await repository.GetCardByIdAsync(request.Id);

        if (card == null)
            throw new AppException("Card not found", HttpStatusCode.NotFound);

        var board = await GetBoardAndCheckAsync(
            request.BoardId,
            userId,
            organizationApiClient.CanChangeWorkspaceAsync,
            boardApiClient.GetBoardAsync
        );


        card.Title = request.Title ?? card.Title;
        card.Description = request.Description ?? card.Description;
        card.Priority = request.Priority;
        card.DueDate = request.DueDate?.ToUniversalTime() ?? card.DueDate;


        card.AssigneeIds = request.AssigneeIds ?? card.AssigneeIds;

        UpdateChecklist(request, card);

        await UpdateLabels(request, board, card);

        await repository.UpdateCardAsync(card);

        var cardResponse = mapper.Map<CardFullResponse>(card);

        await FillUsersAsync(card, cardResponse);

        await eventPublisher.PublishCardUpdatedEmailSend(new ()
        {
            Emails = cardResponse.Watchers.Select(w => w.Email).ToList(),
            Message = $"Задача {card.Title} изменилась"
        });

        return cardResponse;
    }

    public async Task DeleteCardAsync(Guid id, Guid userId)
    {
        var card = await repository.GetCardByIdAsync(id);

        if (card == null)
            throw new AppException("Card not found", HttpStatusCode.NotFound);

        await GetBoardAndCheckAsync(
            id,
            userId,
            organizationApiClient.CanChangeWorkspaceAsync,
            boardApiClient.GetBoardByCardIdAsync
        );

        await repository.DeleteCardAsync(card);
        await PublishFileDeletesAsync([card]);

        await eventPublisher.PublishCardDeletedAsync(new()
        {
            CardId = card.Id
        });
    }


    public async Task WatchCardAsync(Guid cardId, Guid userId)
    {
        var card = await repository.GetCardByIdAsync(cardId);

        if (card == null)
            throw new AppException("Card not found", HttpStatusCode.NotFound);

        await GetBoardAndCheckAsync(
            cardId,
            userId,
            organizationApiClient.CanSeeWorkspaceAsync,
            boardApiClient.GetBoardByCardIdAsync
        );


        card.WatcherIds.Add(userId);

        await repository.UpdateCardAsync(card);
    }

    public async Task UnWatchCardAsync(Guid cardId, Guid userId)
    {
        var card = await repository.GetCardByIdAsync(cardId);

        if (card == null)
            throw new AppException("Card not found", HttpStatusCode.NotFound);

        await GetBoardAndCheckAsync(
            cardId,
            userId,
            organizationApiClient.CanSeeWorkspaceAsync,
            boardApiClient.GetBoardByCardIdAsync
        );


        card.WatcherIds.Remove(userId);

        await repository.UpdateCardAsync(card);
    }


    private async Task UpdateLabels(UpdateCardRequest request, BoardDto board, Card card)
    {
        if (request.Labels == null)
        {
            return;
        }

        var existingLabels =
            await labelRepository.GetLabelsByCardIdsAsync(
                board.Columns.SelectMany(c => c.Cards).Select(x => x.CardId).ToList()
            );

        var newLabels = request.Labels
            .Where(l => !existingLabels.Any(el => el.Name.Equals(l.Name, StringComparison.OrdinalIgnoreCase)))
            .ToList();


        var deletedLabels = card.Labels.Where(l => !request.Labels.Any(el =>
            el.Id == Guid.Empty || el.Name.Equals(l.Name, StringComparison.OrdinalIgnoreCase))).ToList();

        card.Labels = existingLabels.Where(el =>
                request.Labels.Any(l => l.Name.Equals(el.Name, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        foreach (var label in deletedLabels)
        {
            var existLabel = await labelRepository.GetLabelByIdAsync(label.Id);

            if (existLabel == null)
            {
                continue;
            }

            if (existLabel.Cards.Count(c => c.Id != card.Id) == 0)
            {
                await labelRepository.DeleteLabelAsync(existLabel);
            }
        }

        card.Labels.AddRange(newLabels.Select(l => mapper.Map<CardLabel>(l)));
    }

    private void UpdateChecklist(UpdateCardRequest request, Card card)
    {
        if (request.Checklist == null) return;

        card.Checklist ??= new CardChecklist { Title = request.Checklist.Title };
        card.Checklist.Title = request.Checklist.Title;

        var items = card.Checklist.Items;


        foreach (var dto in request.Checklist.Items)
        {
            var item = items.FirstOrDefault(i => i.Id == dto.Id && dto.Id != Guid.Empty)
                       ?? new CardChecklistItem();

            item.Text = dto.Text;
            item.IsCompleted = dto.IsCompleted;
            item.Position = dto.Position;

            if (item.Id == Guid.Empty)
                items.Add(item);
        }

        var actualIds = request.Checklist.Items
            .Where(i => i.Id != Guid.Empty)
            .Select(i => i.Id)
            .ToHashSet();

        items.RemoveAll(i => i.Id != Guid.Empty && !actualIds.Contains(i.Id));
    }

    private async Task FillUsersAsync(Card card, CardFullResponse response)
    {
        if (card.AssigneeIds?.Any() == true)
            response.Assignees = await userApiClient.GetUsers(card.AssigneeIds);

        if (card.WatcherIds?.Any() == true)
            response.Watchers = await userApiClient.GetUsers(card.WatcherIds);
    }


    private async Task<BoardDto> GetBoardAndCheckAsync(
        Guid id,
        Guid userId,
        Func<Guid, Guid, Task<bool>> permissionCheck,
        Func<Guid, Task<BoardDto?>> boardResolver)
    {
        var board = await boardResolver(id)
                    ?? throw new AppException("Board not found", HttpStatusCode.NotFound);

        if (!await permissionCheck(board.WorkspaceId, userId))
            throw new AppException("Access denied", HttpStatusCode.Forbidden);

        return board;
    }

    private async Task PublishFileDeletesAsync(IEnumerable<Card> cards)
    {
        var fileIds = cards
            .SelectMany(c => c.Attachments)
            .Select(a => a.FileId)
            .ToList();

        if (fileIds.Any())
            await eventPublisher.PublishFilesDeletedAsync(new() { FileIds = fileIds });
    }

    private async Task DeleteCardsInternalAsync(
        IEnumerable<Guid> cardIds,
        Func<Task> successEvent,
        Func<Exception, Task> failEvent)
    {
        try
        {
            var cards = await repository.GetCardsByIdsAsync(cardIds.ToList());
            if (!cards.Any()) return;

            await repository.DeleteCardsAsync(cards);
            await PublishFileDeletesAsync(cards);
            await successEvent();
        }
        catch (Exception ex)
        {
            await failEvent(ex);
            throw;
        }
    }
}