using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.DTOs;
using Organization.Application.DTOs.Requestes;
using Organization.Application.Interfaces;

namespace Organization.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class InvitationController : ControllerBase
{
    private readonly IInvitationService _invitationService;

    public InvitationController(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    [HttpPost]
    public async Task<IActionResult> Invite([FromBody] CreateInvitationRequest dto)
    {
        await _invitationService.CreateInvitationAsync(dto.WorkspaceId, dto.Email, dto.Role);
        return Ok();
    }

    [HttpPost("{invitationId:guid}/accept")]
    public async Task<IActionResult> Accept(Guid invitationId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        return Ok(await _invitationService.AcceptInvitationAsync(invitationId, userId));
    }
    
    [HttpGet("{invitationId:guid}/workspace")]
    public async Task<IActionResult> GetByInvitation(Guid invitationId)
    {
        return Ok(await _invitationService.GetWorkspaceByInvitationAsync(invitationId));
    }
}