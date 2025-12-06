using System.Security.Claims;
using BoardService.Application.DTOs;
using BoardService.Application.DTOs.Requestes;
using BoardService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoardService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BoardController : ControllerBase
{
    private readonly IBoardService _boardService;

    public BoardController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBoard([FromBody] CreateBoardRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        return Ok(await _boardService.CreateBoardAsync(userId, request));
    }
    
    
    [HttpGet("{boardId:guid}")]
    public async Task<IActionResult> GetBoard(Guid boardId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        return Ok(await _boardService.GetBoardAsync(userId, boardId));
    }
    
    [HttpGet("{boardId:guid}/full")]
    public async Task<IActionResult> GetBoardFull(Guid boardId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        return Ok(await _boardService.GetBoardFullAsync(userId, boardId));
    }

    [HttpGet("workspace/{workspaceId:guid}")]
    public async Task<IActionResult> GetBoardsByWorkspace(Guid workspaceId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        return Ok(await _boardService.GetBoardsByWorkspaceAsync(userId, workspaceId));
    }

    [HttpGet("{boardId:guid}/column/{columnId:guid}/exist")]
    public async Task<IActionResult> IsExistBoardWithColumn(Guid boardId, Guid columnId)
    {
        return Ok(await _boardService.IsExistBoardWithColumnAsync(boardId, columnId));
    }
    

}