using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using UserProfile.Application.DTOs.Requestes;
using UserProfile.Application.Interfaces;
using UserProfile.Application.Services;

namespace UserProfile.Api.Controllers;
 
[ApiController]
[Route("api/v1/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IUserProfileService _service;

    public ProfileController(IUserProfileService service)
    {
        _service = service;
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetProfile(Guid userId)
    {
        if (!IsYourProfile(userId)) return Forbid();

        return Ok(await _service.GetProfileAsync(userId));
    }

    [HttpGet]
    public async Task<IActionResult> GetMyProfile()
    {
        var jwtUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (jwtUserId == null) return Forbid();

        return Ok(await _service.GetProfileAsync(Guid.Parse(jwtUserId)));
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateMyProfile(UpdateUserProfileDto dto)
    {
        var jwtUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (jwtUserId == null) return Forbid();

        
        return Ok(await _service.UpdateProfileAsync(dto, Guid.Parse(jwtUserId)));
    }
    
    private bool IsYourProfile(Guid userId)
    {
        var jwtUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (jwtUserId == userId.ToString())
        {
            return true;
        }

        return false;
    }

    [HttpPut("preferences")]
    public async Task<IActionResult> UpdatePreferences([FromBody] UserPreferencesUpdateDto dto)
    {
        if (!IsYourProfile(dto.UserId)) return Forbid();

        
        await _service.UpdatePreferencesAsync(dto);
        return NoContent();
    }
}