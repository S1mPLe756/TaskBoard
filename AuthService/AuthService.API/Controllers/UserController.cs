using AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController(IUserService _userService) : ControllerBase
{
        [HttpPost("bulk")]
        public async Task<IActionResult> GetUsers([FromBody] List<Guid> ids)
        {
                return Ok(await _userService.GetUsersByIdsAsync(ids));
        }
        
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUsers(string email)
        {
                return Ok(await _userService.GetUsersByEmailAsync(email));
        }
}