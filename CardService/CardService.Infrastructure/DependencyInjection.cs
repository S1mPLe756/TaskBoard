using CardService.Domain.Interfaces;
using CardService.Infrastructure.ExternalAPI;
using CardService.Infrastructure.ExternalAPI.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Shared.Middleware.Handlers;

namespace CardService.Infrastructure;

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
        
        services.AddTransient<UserIdHeaderHandler>();

        services.AddRefitClient<IBoardApiRefitClient>()
            .ConfigureHttpClient(client => 
            {
                client.BaseAddress = new Uri(configuration["ExternalServices:BoardApi"]);
            }).AddHttpMessageHandler<UserIdHeaderHandler>();
        ;

        services.AddScoped<IBoardApiClient, BoardApiClient>();
        
        services.AddRefitClient<IUserApiRefitClient>()
            .ConfigureHttpClient(client => 
            {
                client.BaseAddress = new Uri(configuration["ExternalServices:AuthApi"]);
            }).AddHttpMessageHandler<UserIdHeaderHandler>();
        ;

        services.AddScoped<IUserApiClient, UserApiClient>();
        
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