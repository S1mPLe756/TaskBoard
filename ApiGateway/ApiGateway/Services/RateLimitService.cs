using Microsoft.Extensions.Caching.Memory;

namespace ApiGateway.Services;

public class RateLimitService
{
    private readonly IMemoryCache _cache;

    public RateLimitService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public bool CheckLimit(string key, int maxAttempts, int periodSeconds)
    {
        if (!_cache.TryGetValue(key, out int attempts))
        {
            attempts = 0;
        }

        if (attempts >= maxAttempts)
        {
            return false;
        }

        _cache.Set(key, attempts + 1, TimeSpan.FromSeconds(periodSeconds));
        return true;
    }

}