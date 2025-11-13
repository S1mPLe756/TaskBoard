using System.IdentityModel.Tokens.Jwt;
using ExceptionService;
using LoggingService.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
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
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
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

        // токен только парсится, без проверки подписи и срока
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            SignatureValidator = (token, parameters) =>
                new JwtSecurityToken(token) // просто парсит токен
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

builder.Services.AddAutoMapper(typeof(AuthMappingProfile).Assembly);
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

app.UseExceptionService();
app.UseTaskBoardLoggingModule();

app.UseHttpsRedirection();
app.UseGatewayUser();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();