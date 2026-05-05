using SubscriptionService.Infrastructure.Settings;

namespace SubscriptionService.Infrastructure.Handlers;

using System.Text;
using Microsoft.Extensions.Options;

/// <summary>
/// Обработчик для Basic аутентификации YooKassa
/// </summary>
public class YooKassaAuthHandler : DelegatingHandler
{
    private readonly IOptions<YooKassaSettings> _settings;

    public YooKassaAuthHandler(IOptions<YooKassaSettings> settings)
    {
        _settings = settings;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var auth = $"{_settings.Value.ShopId}:{_settings.Value.SecretKey}";
        var encodedAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));
        
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encodedAuth);
        
        return await base.SendAsync(request, cancellationToken);
    }
}