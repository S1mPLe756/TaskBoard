using System.IdentityModel.Tokens.Jwt;
using BoardService.Application.Interfaces;
using BoardService.Application.Mappings;
using BoardService.Application.Services;
using BoardService.Domain.Interfaces;
using BoardService.Infrastructure;
using BoardService.Infrastructure.DbContext;
using BoardService.Infrastructure.Messaging.Consumers;
using BoardService.Infrastructure.Messaging.Producers;
using BoardService.Infrastructure.Repositories;
using BoardService.Infrastructure.Settings;
using Confluent.Kafka;
using DotNetEnv;
using ExceptionService;
using Hangfire;
using Hangfire.PostgreSql;
using LoggingService.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
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
        Title = "API Board",
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

builder.Services.AddDbContext<BoardDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTaskBoardLoggingModule(builder.Configuration);
builder.Host.UseSerilog();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IColumnRepository, ColumnRepository>();
builder.Services.AddScoped<ICardRepository, CardRepository>();

builder.Services.AddScoped<IColumnService, ColumnService>();
builder.Services.AddScoped<ICardService, CardService>();

builder.Services.AddAutoMapper(typeof(BoardMapperProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ColumnMapperProfile).Assembly);
builder.Services.AddAutoMapper(typeof(CardMapperProfile).Assembly);

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<KafkaSettings>>().Value);

builder.Services.AddSingleton<IEventPublisher, BoardProducerService>();
builder.Services.AddScoped<IBoardService, BoardService.Application.Services.BoardService>();

builder.Services.AddHostedService<CardCreatedConsumer>();
builder.Services.AddHostedService<BoardCardsDeletedConsumer>();
builder.Services.AddHostedService<BoardCardsDeleteFailedConsumer>();
builder.Services.AddHostedService<ColumnCardsDeletedConsumer>();
builder.Services.AddHostedService<ColumnCardsDeleteFailedConsumer>();
builder.Services.AddHostedService<CardDeletedConsumer>();
builder.Services.AddHangfire(config =>
{
    config.UsePostgreSqlStorage(options =>
    {
        options.UseNpgsqlConnection(
            builder.Configuration.GetConnectionString("Hangfire")
        );

    });
});


builder.Services.AddHangfireServer();


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