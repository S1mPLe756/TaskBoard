using BoardService.Domain.Interfaces;
using BoardService.Infrastructure.ExternalAPI.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace BoardService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRefitClient<IOrganizationApiRefitClient>()
            .ConfigureHttpClient(client => 
            {
                client.BaseAddress = new Uri(configuration["ExternalServices:OrganizationApi"]);
            });

        services.AddScoped<IOrganizationApiClient, OrganizationApiClient>();

        return services;
    }

}