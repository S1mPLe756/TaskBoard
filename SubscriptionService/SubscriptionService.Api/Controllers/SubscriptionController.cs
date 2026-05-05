using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SubscriptionService.Application.DTOs.Responses;
using SubscriptionService.Application.Interfaces;
using SubscriptionService.Domain.Enums;

namespace SubscriptionService.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }
    

    [HttpGet("{cardId:guid}")]
    public async Task<IActionResult> GetSubscription(Guid subscriptionId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        return Ok(await _subscriptionService.GetSubscriptionAsync(subscriptionId, userId));
    }
    
    [HttpPost("with-token")]
    public async Task<ActionResult<ConfirmationResponse>> CreatePayment()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        var result = await _subscriptionService.CreatePaymentAsync(userId);
        return Ok(result);
    }
    
    [HttpPost("with-card/{paymentMethodId}")]
    public async Task<ActionResult<ConfirmationResponse>> CreatePaymentWithCard(string paymentMethodId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        var result = await _subscriptionService.CreatePaymentWithCardAsync(userId, paymentMethodId);
        return Ok(result);
    }

    [HttpPost("with-sbp")]
    public async Task<ActionResult<ConfirmationResponse>> CreatePaymentWithSbp()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        var result = await _subscriptionService.CreatePaymentWithSbpAsync(userId);
        return Ok(result);
    }
    
    [HttpPatch("{subscriptionId}/set-status")]
    public async Task<ActionResult<SubscriptionResponse>> SetStatus(Guid subscriptionId, [FromQuery] SubscriptionStatus status)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        var result = await _subscriptionService.SetSubscriptionStatusAsync(userId, subscriptionId, status);
        return Ok(result);
    }

}