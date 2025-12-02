using ApiGateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class HealthController(HealthAggregatorService healthAggregator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await healthAggregator.CheckHealthAsync();

        if (result.Services.Any(s => !s.IsHealthy))
            return StatusCode(503, result);

        return Ok(result);
    }

}