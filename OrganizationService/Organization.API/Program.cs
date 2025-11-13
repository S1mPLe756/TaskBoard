using System.IdentityModel.Tokens.Jwt;
using ExceptionService;
using LoggingService.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Organization.Application.Interfaces;
using Organization.Application.Mappings;
using Organization.Application.Services;
using Organization.Domain.Interfaces;
using Organization.Infrastructure.DbContext;
using Organization.Infrastructure.Repositories;
using Serilog;
using Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Organization",
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

builder.Services.AddDbContext<OrganizationDbContext>(opt =>
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

builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();

builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();
builder.Services.AddScoped<IInvitationService, InvitationService>();

builder.Services.AddAutoMapper(typeof(WorkspaceMapperProfile).Assembly);
builder.Services.AddAutoMapper(typeof(InvitationMapperProfile).Assembly);

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