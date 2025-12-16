using System.Net;
using AutoMapper;
using CardService.Application.DTOs.Responses;
using CardService.Application.Interfaces;
using CardService.Domain.Interfaces;
using ExceptionService;
using Microsoft.AspNetCore.Http;

namespace CardService.Application.Services;

public class CardAttachmentsService(
    ICardRepository repository,
    IMapper mapper,
    IBoardApiClient boardApiClient,
    IOrganizationApiClient organizationApiClient,
    IFileApiClient fileApiClient) : ICardAttachmentsService
{
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


        card.Attachments.Add(new()
        {
            ContentType = file.ContentType,
            FileName = file.FileName,
            FileId = response.Id
        });

        await repository.UpdateCardAsync(card);

        return mapper.Map<CardFullResponse>(card);
    }


    public async Task DeleteAttachmentAsync(Guid cardId, string fileId, Guid userId)
    {
        var card = await repository.GetCardByIdAsync(cardId);

        if (card == null)
            throw new AppException("Card not found", HttpStatusCode.NotFound);

        var board = await boardApiClient.GetBoardByCardIdAsync(cardId);

        if (!await organizationApiClient.CanChangeWorkspaceAsync(board.WorkspaceId, userId))
        {
            throw new AppException("You can't change workspace", HttpStatusCode.Forbidden);
        }

        var file = card.Attachments.FirstOrDefault(x => x.FileId == fileId);

        if (file == null)
        {
            throw new AppException("Attachment not found", HttpStatusCode.NotFound);
        }

        await fileApiClient.DeleteFileAsync(fileId);


        card.Attachments.Remove(file);

        await repository.UpdateCardAsync(card);
    }
}