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

    [HttpPost("batch")]
    public async Task<IActionResult> GetCards([FromBody] GetCardsBatchRequest request)
    {
        return Ok(await _cardService.GetCardsBatchAsync(request));
    }
}