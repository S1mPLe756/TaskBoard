using System.Security.Claims;
using BoardService.Application.DTOs.Requestes;
using BoardService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoardService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CardPositionController(ICardService cardService) : ControllerBase
{
    [HttpPatch]
    public async Task<IActionResult> UpdateCardPositionBoard([FromBody] UpdateCardPositionRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        return Ok(await cardService.UpdateCardPositionAsync(userId, request));
    }
}