using Microsoft.Extensions.Logging;

namespace SubscriptionService.Infrastructure.Handlers;

public class YooKassaIdempotencyHandler : DelegatingHandler
{
    private readonly ILogger<YooKassaIdempotencyHandler> _logger;

    public YooKassaIdempotencyHandler(ILogger<YooKassaIdempotencyHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Method == HttpMethod.Post && !request.Headers.Contains("Idempotence-Key"))
        {
            var idempotenceKey = Guid.NewGuid().ToString();
            request.Headers.TryAddWithoutValidation("Idempotence-Key", idempotenceKey);
            
            _logger.LogDebug("Added Idempotence-Key: {IdempotenceKey} for {Method} {Url}", 
                idempotenceKey, request.Method, request.RequestUri);
        }
        
        return await base.SendAsync(request, cancellationToken);
    }
}