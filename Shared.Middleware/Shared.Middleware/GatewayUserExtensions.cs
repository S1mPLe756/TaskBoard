using Microsoft.AspNetCore.Builder;

namespace Shared.Middleware;

public static class GatewayUserExtensions
{
    public static IApplicationBuilder UseGatewayUser(this IApplicationBuilder app)
    {
        app.UseMiddleware<GatewayUserMiddleware>();
        return app;
    }
}