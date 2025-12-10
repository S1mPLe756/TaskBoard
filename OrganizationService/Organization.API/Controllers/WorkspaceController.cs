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
    public async Task<IActionResult> GetWorkspace(Guid workspaceId)
    {
        var workspace = await _workspaceService.GetWorkspaceAsync(workspaceId);
        return Ok(workspace);
    }
    
    [HttpDelete("{workspaceId:guid}")]
    public async Task<IActionResult> DeleteWorkspace(Guid workspaceId)
    { 
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        await _workspaceService.DeleteWorkspaceAsync(workspaceId, userId);
        return NoContent();
    }
    
    [HttpPatch("{workspaceId:guid}")]
    public async Task<IActionResult> ChangeWorkspace(Guid workspaceId, ChangeWorkspaceRequest request)
    { 
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        await _workspaceService.ChangeWorkspaceAsync(request, workspaceId, userId);
        return NoContent();
    }
    
    [HttpGet("{workspaceId:guid}/can-change-workspace/{userId:guid}")]
    public async Task<IActionResult> CanChangeWorkspace(Guid workspaceId, Guid userId)
    {
        var workspace = await _workspaceService.CanChangeWorkspaceAsync(userId, workspaceId);
        return Ok(workspace);
    }
    
    [HttpGet("{workspaceId:guid}/can-change-workspace")]
    public async Task<IActionResult> CanChangeWorkspace(Guid workspaceId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var workspace = await _workspaceService.CanChangeWorkspaceAsync(userId, workspaceId);
        return Ok(workspace);
    }
    
    [HttpGet("{workspaceId:guid}/can-see-workspace/{userId:guid}")]
    public async Task<IActionResult> CanSeeWorkspace(Guid workspaceId, Guid userId)
    {
        var workspace = await _workspaceService.CanSeeWorkspace(userId, workspaceId);
        return Ok(workspace);
    }

    [HttpPost("{workspaceId:guid}/members")]
    public async Task<IActionResult> AddMember(Guid workspaceId, [FromBody] AddMemberRequest dto)
    {
        var member = await _workspaceService.AddMemberAsync(workspaceId, dto.UserId, dto.Role);
        return Ok(member);
    }
    
    [HttpGet("{workspaceId:guid}/members")]
    public async Task<IActionResult> GetMembers(Guid workspaceId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var member = await _workspaceService.GetMembersAsync(workspaceId, userId);
        return Ok(member);
    }
}