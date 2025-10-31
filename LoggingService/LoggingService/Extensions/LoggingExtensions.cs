using Castle.DynamicProxy;
using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Elastic.Transport;
using LoggingService.Filters;
using LoggingService.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LoggingService.Extensions;

public static class LoggingExtensions
{
    public static IServiceCollection AddTaskBoardLoggingModule(this IServiceCollection services, IConfiguration config)
    {
        var esUri = config["Elastic:Uri"] ?? "https://localhost:9201";
        var esUser = config["Elastic:Username"] ?? "elastic";
        var esPass = config["Elastic:Password"] ?? "changeme";

        // Настройка Serilog
        Log.Logger = new LoggerConfiguration().Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Elasticsearch([new Uri(esUri)], opts =>
            {
                opts.DataStream = new DataStreamName("logs", "task-board", "demo");
                opts.ChannelDiagnosticsCallback = (channel) => Console.Write(channel.ObservedException);
                opts.BootstrapMethod = BootstrapMethod.Failure;
                opts.ConfigureChannel = channelOpts =>
                {
                    channelOpts.BufferOptions = new BufferOptions
                    {
                        ExportMaxConcurrency = 10
                    };
                };
            }, transport =>
            {
                transport.Authentication(new BasicAuthentication(esUser, esPass));
                transport.ServerCertificateValidationCallback((o, cert, chain, errors) => true);
                transport.RequestTimeout(TimeSpan.FromSeconds(10));
                transport.DisableDirectStreaming();
                transport.EnableDebugMode();
                transport.DisablePing();
            }).CreateLogger();
        Serilog.Debugging.SelfLog.Enable(msg => Console.Error.WriteLine(msg));

        // AOP Interceptor
        services.AddSingleton<ProxyGenerator>();
        services.AddTransient<LoggingInterceptor>();

        // Контроллерные фильтры
        services.AddControllers(o => o.Filters.Add<LoggingActionFilter>());

        return services;
    }
    
    public static IApplicationBuilder UseTaskBoardLoggingModule(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
        return app;
    }

}