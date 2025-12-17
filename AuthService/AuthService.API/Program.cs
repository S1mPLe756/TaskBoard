using AuthService.Application.Interfaces;
using AuthService.Application.Mapping;
using AuthService.Application.Services;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.DbContext;
using AuthService.Infrastructure.Messaging.Producers;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Services;
using Confluent.Kafka;
using DotNetEnv;
using ExceptionService;
using LoggingService.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Auth",
        Version = "v1",
    });
});

Env.Load();
builder.Configuration.AddEnvironmentVariables();


builder.Services.AddDbContext<AuthDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTaskBoardLoggingModule(builder.Configuration);
builder.Host.UseSerilog();

builder.Services.AddHealthChecks()
    .AddKafka(
        config: new ProducerConfig()
        {
            BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
            RequestTimeoutMs = 5
        },
        name: "KafkaBroker",
        timeout: TimeSpan.FromSeconds(5),
        failureStatus: HealthStatus.Unhealthy,
        tags: ["message-broker"]
    )
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!, name: "Postgres Auth DB");
Console.WriteLine(builder.Configuration["Kafka:BootstrapServers"]);
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService.Application.Services.AuthService>();
builder.Services.AddSingleton<IEventPublisher, KafkaProducerService>();

builder.Services.AddAutoMapper(typeof(AuthMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
builder.Services.AddSingleton(builder.Configuration["Kafka:BootstrapServers"]!);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/api/v1/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        
        var result = new
        {
            status = report.Status.ToString(),
            services = report.Entries.Select(e => new
            {
                serviceName = e.Key,
                status = e.Value.Status.ToString(),
                duration = e.Value.Duration.ToString(),
                description = e.Value.Status != HealthStatus.Healthy ? e.Key + " is unreachable" : e.Value.Description,
            })
        };
        await context.Response.WriteAsJsonAsync(result);
    }
});

app.UseExceptionService();
app.UseTaskBoardLoggingModule();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();