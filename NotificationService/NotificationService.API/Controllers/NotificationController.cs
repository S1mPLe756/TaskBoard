using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;

namespace NotificationService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _service;

    public NotificationController(INotificationService service)
    {
        _service = service;
    }

    [HttpPost("email")]
    public async Task<IActionResult> SendEmail([FromBody] NotificationRequest request)
    {
        var id = await _service.SendAsync(request);
        return Ok(new { NotificationId = id });
    }
}