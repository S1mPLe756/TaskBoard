using System.IdentityModel.Tokens.Jwt;
using ExceptionService;
using LoggingService.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Mappings;
using NotificationService.Domain.Interfaces;
using NotificationService.Infrastructure.DbContext;
using NotificationService.Infrastructure.Messaging.Consumers;
using NotificationService.Infrastructure.Repositories;
using NotificationService.Infrastructure.Senders;
using NotificationService.Infrastructure.Settings;
using Serilog;
using Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Notification",
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

builder.Services.AddDbContext<NotificationDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));

builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped<INotificationService, NotificationService.Application.Services.NotificationService>();

builder.Services.AddAutoMapper(typeof(NotificationMapperProfile).Assembly);

builder.Services.AddHostedService<NotificationEmailSendConsumer>();

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