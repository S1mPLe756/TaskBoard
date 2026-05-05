using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;
using SubscriptionService.Infrastructure.ExternalAPI;
using SubscriptionService.Infrastructure.ExternalAPI.Clients;
using SubscriptionService.Infrastructure.Handlers;
using SubscriptionService.Infrastructure.Settings;

namespace SubscriptionService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<YooKassaSettings>(configuration.GetSection("YooKassa"));

        services.AddRefitClient<IYookassaApiRefitClient>()
            .ConfigureHttpClient((serviceProvider, client) => 
            {
                var settings = serviceProvider.GetRequiredService<IOptions<YooKassaSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddHttpMessageHandler<YooKassaAuthHandler>()
            .AddHttpMessageHandler<YooKassaIdempotencyHandler>()
            .AddHttpMessageHandler<YooKassaLoggingHandler>();

        services.AddScoped<IYookassaApiRefitClient, YookassaApiClient>();


        return services;
    }

}