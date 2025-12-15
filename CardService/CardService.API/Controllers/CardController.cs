using System.Security.Claims;
using CardService.Application.DTOs;
using CardService.Application.DTOs.Requests;
using CardService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CardService.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CardController : ControllerBase
{
    private readonly ICardService _cardService;

    public CardController(ICardService cardService)
    {
        _cardService = cardService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCard([FromBody] CreateCardRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        return Ok(await _cardService.CreateCard(request, userId));
    }
    
    [HttpPatch]
    public async Task<IActionResult> UpdateCard([FromBody] UpdateCardRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        return Ok(await _cardService.UpdateCard(request, userId));
    }
    
    [HttpPost("{cardId:guid}/attachments")]
    public async Task<IActionResult> AddAttachment(Guid cardId, [FromForm] IFormFile file)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        return Ok(await _cardService.AddAttachmentAsync(cardId, file, userId));
    }


    [HttpPost("batch")]
    public async Task<IActionResult> GetCards([FromBody] GetCardsBatchRequest request)
    {
        return Ok(await _cardService.GetCardsBatchAsync(request));
    }

    [HttpGet("{cardId:guid}")]
    public async Task<IActionResult> GetCard(Guid cardId)
    {
        return Ok(await _cardService.GetCard(cardId));
    }
}