using System.IdentityModel.Tokens.Jwt;
using Confluent.Kafka;
using DotNetEnv;
using ExceptionService;
using LoggingService.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Shared.Middleware;
using UserProfile.Application.Interfaces;
using UserProfile.Application.Mapping;
using UserProfile.Application.Services;
using UserProfile.Domain.Interfaces;
using UserProfile.Infrastructure.DbContext;
using UserProfile.Infrastructure.Messaging.Consumers;
using UserProfile.Infrastructure.Repositories;
using UserProfile.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

Env.Load();
builder.Configuration.AddEnvironmentVariables();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!, name: "Postgres UserProfile DB");

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Profile",
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

builder.Services.AddAuthorization();

builder.Services.AddDbContext<UserProfileDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTaskBoardLoggingModule(builder.Configuration);
builder.Host.UseSerilog();

builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IUserPreferencesRepository, UserPreferencesRepository>();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

builder.Services.AddScoped<IUserProfileService, UserProfileService>();

builder.Services.AddAutoMapper(typeof(ProfileMappingProfile).Assembly);
builder.Services.AddHostedService<UserRegisteredConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
app.UseGatewayUser();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();