using System.Security.Claims;
using CardService.Application.Interfaces;
using Elastic.CommonSchema;
using Microsoft.AspNetCore.Mvc;

namespace CardService.API.Controllers;

[ApiController]
[Route("api/v1/Card/")]
public class CardAttachmentsController(ICardAttachmentsService cardAttachmentsService) : ControllerBase
{
    
    
    [HttpPost("{cardId:guid}/attachments")]
    public async Task<IActionResult> AddAttachment(Guid cardId, [FromForm] IFormFile file)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        return Ok(await cardAttachmentsService.AddAttachmentAsync(cardId, file, userId));
    }
    
    [HttpDelete("{cardId:guid}/attachments/{fileId}")]
    public async Task<IActionResult> AddAttachment(Guid cardId, string fileId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        await cardAttachmentsService.DeleteAttachmentAsync(cardId, fileId, userId);
        return NoContent();
    }

}