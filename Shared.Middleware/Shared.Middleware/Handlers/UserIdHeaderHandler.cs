using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Shared.Middleware.Handlers;

public class UserIdHeaderHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserIdHeaderHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor
            .HttpContext?
            .User?
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value;

        if (!string.IsNullOrEmpty(userId))
        {
            request.Headers.Add("x-User-Id", userId);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}