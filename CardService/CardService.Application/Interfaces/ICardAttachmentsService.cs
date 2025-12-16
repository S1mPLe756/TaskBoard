using CardService.Application.DTOs.Responses;
using Microsoft.AspNetCore.Http;

namespace CardService.Application.Interfaces;

public interface ICardAttachmentsService
{
    Task<CardFullResponse> AddAttachmentAsync(Guid cardId, IFormFile file, Guid userId);
    Task DeleteAttachmentAsync(Guid cardId, string fileId, Guid userId);
}