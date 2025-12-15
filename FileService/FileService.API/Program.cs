using System.IdentityModel.Tokens.Jwt;
using Confluent.Kafka;
using DotNetEnv;
using ExceptionService;
using FileService.Domain.Interfaces;
using FileService.Infrastructure.Messaging;
using FileService.Infrastructure.Settings;
using LoggingService.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Serilog;
using Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

Env.Load();
builder.Configuration.AddEnvironmentVariables();

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
    );

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Card",
        Version = "v1",
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            SignatureValidator = (token, parameters) =>
                new JwtSecurityToken(token) 
        };
    });


builder.Services.AddTaskBoardLoggingModule(builder.Configuration);
builder.Host.UseSerilog();
builder.Services.AddHttpContextAccessor();


builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IConfiguration>()
        .GetSection("MongoSettings").Get<MongoSettings>();
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddScoped(sp =>
{
    var settings = sp.GetRequiredService<IConfiguration>()
        .GetSection("MongoSettings").Get<MongoSettings>();
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});



builder.Services.AddScoped<IFileService, FileService.Infrastructure.Services.FileService>();
builder.Services.AddSingleton<IEventPublisher, FileKafkaProducerService>();


builder.Services.AddSingleton(builder.Configuration["Kafka:BootstrapServers"]!);
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<KafkaSettings>>().Value);




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

app.UseExceptionService();
app.UseTaskBoardLoggingModule();

app.UseHttpsRedirection();

app.UseGatewayUser();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();