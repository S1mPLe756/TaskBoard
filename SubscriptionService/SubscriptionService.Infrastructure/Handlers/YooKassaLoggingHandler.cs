using Microsoft.Extensions.Logging;

namespace SubscriptionService.Infrastructure.Handlers;

public class YooKassaLoggingHandler : DelegatingHandler
{
    private readonly ILogger<YooKassaLoggingHandler> _logger;

    public YooKassaLoggingHandler(ILogger<YooKassaLoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Логирование запроса
        _logger.LogInformation("YooKassa API Request: {Method} {Url}", request.Method, request.RequestUri);
        
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            if (request.Content != null)
            {
                var requestBody = await request.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogDebug("Request Body: {RequestBody}", requestBody);
            }
        }
        
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            stopwatch.Stop();
            
            // Логирование ответа
            _logger.LogInformation("YooKassa API Response: {StatusCode} for {Method} {Url} in {ElapsedMs}ms", 
                (int)response.StatusCode, request.Method, request.RequestUri, stopwatch.ElapsedMilliseconds);
            
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogDebug("Response Body: {ResponseBody}", responseBody);
            }
            
            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "YooKassa API Request failed for {Method} {Url} after {ElapsedMs}ms", 
                request.Method, request.RequestUri, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}
