using System.Security.Claims;
using BoardService.Application.DTOs;
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
}