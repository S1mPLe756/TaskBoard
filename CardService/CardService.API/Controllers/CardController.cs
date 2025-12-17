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
        
        return Ok(await _cardService.CreateCardAsync(request, userId));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> CreateCard(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        await _cardService.DeleteCardAsync(id, userId);
        return NoContent();
    }
    
    [HttpPatch]
    public async Task<IActionResult> UpdateCard([FromBody] UpdateCardRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        return Ok(await _cardService.UpdateCardAsync(request, userId));
    }


    [HttpPost("batch")]
    public async Task<IActionResult> GetCards([FromBody] GetCardsBatchRequest request)
    {
        return Ok(await _cardService.GetCardsBatchAsync(request));
    }

    [HttpGet("{cardId:guid}")]
    public async Task<IActionResult> GetCard(Guid cardId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        return Ok(await _cardService.GetCardAsync(cardId, userId));
    }
    
    [HttpPatch("{cardId:guid}/watch")]
    public async Task<IActionResult> WatchCard(Guid cardId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        await _cardService.WatchCardAsync(cardId, userId);
        return NoContent();
    }
    
    [HttpPatch("{cardId:guid}/unwatch")]
    public async Task<IActionResult> UnWatchCard(Guid cardId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        await _cardService.UnWatchCardAsync(cardId, userId);
        return NoContent();
    }
}