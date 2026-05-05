using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionService.Application.DTOs.Responses;
using SubscriptionService.Application.Interfaces;
using SubscriptionService.Domain.Entities;

namespace SubscriptionService.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserCardController: ControllerBase
{
    private readonly IUserCardService _userCardService;

    public UserCardController(IUserCardService userCardService)
    {
        _userCardService = userCardService;
    }
    
    [HttpPost("bind")]
    public async Task<ActionResult<ConfirmationResponse>> BindCard()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        var result = await _userCardService.BindCardAsync(userId);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<UserCard>>> GetUserCards()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        var result = await _userCardService.GetUserCardsAsync(userId);
        return Ok(result);
    }
    
    [HttpDelete("{cardId}")]
    public async Task<IActionResult> DeleteCard(Guid cardId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        await _userCardService.DeleteCardAsync(userId, cardId);
        return NoContent(); // 204 No Content (аналог void в Spring)
    }
    
    
    [HttpPost("webhook/yookassa")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> YookassaWebhook([FromBody] Dictionary<string, object> payload)
    {
        try
        {
            await _userCardService.ProcessYookassaWebhookAsync(payload);
            return Ok("OK");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}