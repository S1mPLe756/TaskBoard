using System.Security.Claims;
using BoardService.Application.DTOs;
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
    
    [HttpPost("{boardId:guid}")]
    public async Task<IActionResult> CreateColumnBoard([FromBody] CreateColumnBoardRequest request, [FromRoute] Guid boardId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        return Ok(await _columnService.CreateColumnForBoardAsync(request, boardId, userId));
    }
}