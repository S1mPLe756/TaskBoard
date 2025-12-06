using AuthService.Application.DTOs.Requestes;
using AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;   
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refresh)
    {
        return Ok(await _tokenService.RefreshToken(refresh));
    }
    
    [HttpPost("validate")]
    public async Task<IActionResult> Validate([FromBody] string token)
    {
        await _tokenService.Validate(token);
        return Ok();
    }
}