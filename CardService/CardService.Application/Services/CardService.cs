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
    IFileApiClient fileApiClient,
    IEventPublisher eventPublisher)
    : ICardService
{
    public async Task<CardFullResponse> GetCard(Guid cardId)
    {
        var card = await repository.GetCardByIdAsync(cardId);

        if (card == null)
        {
            throw new AppException("Card not found", HttpStatusCode.NotFound);
        }
        var cardResponse = mapper.Map<CardFullResponse>(card);
        
        
        if(card.AssigneeIds.Count != 0)
        {
            var users = await userApiClient.GetUsers(card.AssigneeIds);
            cardResponse.Assignees = users;
        }        
        else if (card.WatcherIds.Count != 0)
        {
            var users = await userApiClient.GetUsers(card.WatcherIds);
            cardResponse.Watchers = users;
        }

        return cardResponse;
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
        
        if(card.DueDate != null)
        {
            card.DueDate = DateTime.SpecifyKind(card.DueDate.Value, DateTimeKind.Utc);
        }    
        
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

    public async Task<CardFullResponse> UpdateCard(UpdateCardRequest request, Guid userId)
    {
        var card = await repository.GetCardByIdAsync(request.Id);

        if (card == null)
            throw new AppException("Card not found", HttpStatusCode.NotFound);
        
        var board = await boardApiClient.GetBoardAsync(request.BoardId);
        
        if (!await organizationApiClient.CanChangeWorkspaceAsync(board.WorkspaceId, userId))
        {
            throw new AppException("You can't change workspace", HttpStatusCode.Forbidden);
        }


        card.Title = request.Title ?? card.Title;
        card.Description = request.Description ?? card.Description;
        card.Priority = request.Priority;
        card.DueDate = request.DueDate ?? card.DueDate;

        if (card.DueDate != null)
        {
            card.DueDate = DateTime.SpecifyKind(card.DueDate.Value, DateTimeKind.Utc);
        }
        
        card.AssigneeIds = request.AssigneeIds ?? card.AssigneeIds;

        UpdateChecklist(request, card);
        
        await UpdateLabels(request, board, card);
        
        await repository.UpdateCardAsync(card);

        var cardResponse = mapper.Map<CardFullResponse>(card);

        if (card.AssigneeIds?.Count > 0)
        {
            var assignees = await userApiClient.GetUsers(card.AssigneeIds);
            cardResponse.Assignees = assignees;
        }

        if (card.WatcherIds?.Count > 0)
        {
            var watchers = await userApiClient.GetUsers(card.WatcherIds);
            cardResponse.Watchers = watchers;
        }

        return cardResponse;

    }
    
    public async Task<CardFullResponse> AddAttachmentAsync(Guid cardId, IFormFile file, Guid userId)
    {
        if (file == null || file.Length == 0)
            throw new AppException("Файл не выбран");
        
        var card = await repository.GetCardByIdAsync(cardId);

        if (card == null)
            throw new AppException("Card not found", HttpStatusCode.NotFound);
        
        var board = await boardApiClient.GetBoardByCardIdAsync(cardId);
        
        if (!await organizationApiClient.CanChangeWorkspaceAsync(board.WorkspaceId, userId))
        {
            throw new AppException("You can't change workspace", HttpStatusCode.Forbidden);
        }

        using var content = new MultipartFormDataContent();
        using var fileStream = file.OpenReadStream();

        var response = await fileApiClient.UploadFileAsync(fileStream, file.FileName, file.ContentType);
        


        card.Attachments.Add(new ()
        {
            ContentType = file.ContentType,
            FileName = file.FileName,
            FileId = response.Id
        });

        await repository.UpdateCardAsync(card);

        return mapper.Map<CardFullResponse>(card);
    }


    private async Task UpdateLabels(UpdateCardRequest request, BoardDto board, Card card)
    {
        if (request.Labels != null)
        {
            var existingLabels =
                await labelRepository.GetLabelsByCardIdsAsync(
                    board.Columns.SelectMany(c => c.Cards).Select(x => x.CardId).ToList()
                );

            var newLabels = request.Labels
                .Where(l => !existingLabels.Any(el => el.Name.Equals(l.Name, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            
            
            var deletedLabels = card.Labels.Where(l => !request.Labels.Any(el => el.Id == Guid.Empty || el.Name.Equals(l.Name, StringComparison.OrdinalIgnoreCase))).ToList();

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
    }

    private void UpdateChecklist(UpdateCardRequest request, Card card)
    {
        if (request.Checklist != null)
        {
            if (card.Checklist == null || card.Checklist.Id != request.Checklist.Id)
            {
                card.Checklist = new CardChecklist()
                {
                    Title = request.Checklist.Title,
                };
            }
   
            foreach (var itemDto in request.Checklist.Items)
            {
                if (itemDto.Id == Guid.Empty)
                {
                    // Новый элемент
                    card.Checklist.Items.Add(new()
                    {
                        Text = itemDto.Text,
                        IsCompleted = itemDto.IsCompleted,
                        Position = itemDto.Position,
                    });
                }
                else
                {
                    var existingItem = card.Checklist.Items.FirstOrDefault(c => c.Id == itemDto.Id);
                    if (existingItem != null)
                    {
                        existingItem.Text = itemDto.Text;
                        existingItem.IsCompleted = itemDto.IsCompleted;
                        existingItem.Position = itemDto.Position;
                    }
                    else
                    {
                        card.Checklist.Items.Add(new()
                        {
                            Id = itemDto.Id,
                            Text = itemDto.Text,
                            IsCompleted = itemDto.IsCompleted,
                            Position = itemDto.Position
                        });
                    }
                }
            }
            
            var idsInRequest = request.Checklist.Items
                .Where(r => r.Id != Guid.Empty)
                .Select(r => r.Id)
                .ToHashSet();

            card.Checklist.Items.RemoveAll(c => c.Id != Guid.Empty && !idsInRequest.Contains(c.Id));
        
        }
    }
}