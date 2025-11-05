using Microsoft.AspNetCore.Mvc;

namespace TaskService.API.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class TaskController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Register()
    {
        return Ok();
    }
}