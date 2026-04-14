using System.Security.Claims;
using CardService.Application.DTOs.Requests;
using CardService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CardService.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    
    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        return Ok(await _commentService.CreateCommentAsync(request, userId));
    }
    
    [HttpGet("card/{cardId:guid}")]
    public async Task<IActionResult> GetComments(Guid cardId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        return Ok(await _commentService.GetCommentsByCardAsync(cardId, userId));
    }
    
    [HttpDelete("{commentId:guid}")]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        await _commentService.DeleteComment(commentId, userId);
        return NoContent();
    }

}