using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Shared.Middleware.Handlers;
using UserProfile.Domain.Interfaces;
using UserProfile.Infrastructure.ExternalAPI;
using UserProfile.Infrastructure.ExternalAPI.Clients;

namespace UserProfile.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<UserIdHeaderHandler>();

        services.AddRefitClient<IFileApiRefitClient>()
            .ConfigureHttpClient(client => 
            {
                client.BaseAddress = new Uri(configuration["ExternalServices:FileApi"]);
            }).AddHttpMessageHandler<UserIdHeaderHandler>();
        ;

        services.AddScoped<IFileApiClient, FileApiClient>();


        return services;
    }

}