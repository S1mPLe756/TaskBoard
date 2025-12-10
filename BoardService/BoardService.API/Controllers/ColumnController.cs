using System.Security.Claims;
using BoardService.Application.DTOs;
using BoardService.Application.DTOs.Requestes;
using BoardService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoardService.API.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class ColumnController : ControllerBase
{
    private IColumnService _columnService;

    public ColumnController(IColumnService columnService)
    {
        _columnService = columnService;
    }
    
    [HttpPost("board/{boardId:guid}")]
    public async Task<IActionResult> CreateColumnBoard([FromBody] CreateColumnBoardRequest request, [FromRoute] Guid boardId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        return Ok(await _columnService.CreateColumnForBoardAsync(request, boardId, userId));
    }
    
    [HttpDelete("{columnId:guid}")]
    public async Task<IActionResult> DeleteColumnBoard([FromRoute] Guid columnId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        await _columnService.DeleteColumnAsync(columnId, userId);
        return Ok();
    }
    
        
    [HttpPatch("{columnId:guid}")]
    public async Task<IActionResult> UpdateColumnBoard([FromRoute] Guid columnId, [FromBody] UpdateColumnBoardRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        return Ok(await _columnService.UpdateColumnAsync(columnId, userId, request));
    }

    [HttpGet("{boardId:guid}")]
    public async Task<IActionResult> GetColumnsBoard([FromRoute] Guid boardId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        return Ok(await _columnService.GetColumnsBoard(boardId, userId));

    }
}