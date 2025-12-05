using System.IdentityModel.Tokens.Jwt;
using CardService.Application.Interfaces;
using CardService.Application.Mappings;
using CardService.Domain.Interfaces;
using CardService.Infrastructure;
using CardService.Infrastructure.DbContext;
using CardService.Infrastructure.Messaging;
using CardService.Infrastructure.Repositories;
using Confluent.Kafka;
using DotNetEnv;
using ExceptionService;
using LoggingService.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
    )
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!, name: "Postgres Boards DB");

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

builder.Services.AddDbContext<CardDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTaskBoardLoggingModule(builder.Configuration);
builder.Host.UseSerilog();
builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ICardRepository, CardRepository>();

builder.Services.AddScoped<ICardService, CardService.Application.Services.CardService>();
builder.Services.AddSingleton<IEventPublisher, KafkaProducerService>();

builder.Services.AddAutoMapper(typeof(CardMapperProfile).Assembly);

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

app.UseExceptionService();
app.UseTaskBoardLoggingModule();

app.UseHttpsRedirection();

app.UseGatewayUser();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();