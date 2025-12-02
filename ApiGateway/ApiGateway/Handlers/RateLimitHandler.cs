using ApiGateway.Services;

namespace ApiGateway.Handlers;

public class RateLimitHandler(RateLimitService rateLimitService, IHttpContextAccessor httpContextAccessor)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var ip = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "local";
        ip += "_" + request.RequestUri;
        if (!rateLimitService.CheckLimit(ip, 5, 60))
        {
            return new HttpResponseMessage((System.Net.HttpStatusCode)429)
            {
                Content = new StringContent("Too many requests")
            };
        }

        return await base.SendAsync(request, cancellationToken);
    }
}