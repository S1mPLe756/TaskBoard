using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Shared.Middleware;

public class GatewayUserMiddleware
{
    private readonly RequestDelegate _next;

    public GatewayUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userId = context.Request.Headers["X-User-Id"].FirstOrDefault();
        if (!string.IsNullOrEmpty(userId))
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            var identity = new ClaimsIdentity(claims, "Gateway");
            context.User = new ClaimsPrincipal(identity);
        }

        await _next(context);
    }
}
