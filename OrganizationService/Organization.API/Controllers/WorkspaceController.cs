using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.DTOs;
using Organization.Application.DTOs.Requestes;
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
    
    [HttpGet("my")]
    public async Task<IActionResult> GetMyWorkspaces()
    {
        var ownerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var workspace = await _workspaceService.GetUserWorkspacesAsync(ownerId);
        return Ok(workspace);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkspaceRequest request)
    {
        var ownerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var workspace = await _workspaceService.CreateWorkspaceAsync(ownerId, request);
        return Ok(workspace);
    }

    [HttpGet("{workspaceId:guid}")]
    public async Task<IActionResult> Get(Guid workspaceId)
    {
        var workspace = await _workspaceService.GetWorkspaceAsync(workspaceId);
        return Ok(workspace);
    }

    [HttpPost("{workspaceId:guid}/members")]
    public async Task<IActionResult> AddMember(Guid workspaceId, [FromBody] AddMemberRequest dto)
    {
        var member = await _workspaceService.AddMemberAsync(workspaceId, dto.UserId, dto.Role);
        return Ok(member);
    }
}