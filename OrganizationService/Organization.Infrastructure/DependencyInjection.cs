using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Organization.Domain.Interfaces;
using Organization.Infrastructure.ExternalAPI;
using Organization.Infrastructure.ExternalAPI.Clients;
using Refit;

namespace Organization.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRefitClient<IUserApiRefitClient>()
            .ConfigureHttpClient(client => 
            {
                client.BaseAddress = new Uri(configuration["ExternalServices:AuthApi"]);
            });

        services.AddScoped<IUserApiClient, UserApiClient>();

        return services;
    }
}