using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.DTOs;
using Organization.Application.Interfaces;

namespace Organization.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class WorkspaceController : ControllerBase
{
    private readonly IWorkspaceService _workspaceService;

    public WorkspaceController(IWorkspaceService workspaceService)
    {
        _workspaceService = workspaceService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] string name)
    {
        var ownerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var workspace = await _workspaceService.CreateWorkspaceAsync(ownerId, name);
        return Ok(workspace);
    }

    [HttpGet("{workspaceId:guid}")]
    public async Task<IActionResult> Get(Guid workspaceId)
    {
        var workspace = await _workspaceService.GetWorkspaceAsync(workspaceId);
        return Ok(workspace);
    }

    [HttpPost("{workspaceId:guid}/members")]
    public async Task<IActionResult> AddMember(Guid workspaceId, [FromBody] AddMemberRequestDto dto)
    {
        var member = await _workspaceService.AddMemberAsync(workspaceId, dto.UserId, dto.Role);
        return Ok(member);
    }
}