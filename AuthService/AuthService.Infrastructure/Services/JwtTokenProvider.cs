using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using AuthService.Application.Interfaces;
using AuthService.Application.Utils;
using AuthService.Domain.Entities;
using ExceptionService;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Infrastructure.Services;

public class JwtTokenProvider : IJwtTokenProvider
{
    private readonly IConfiguration _config;

    public JwtTokenProvider(IConfiguration config)
    {
        _config = config;
    }

    public bool ValidateToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
        try
        {
            handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key),
            }, out _);
            
            return true;
        }
        catch (Exception e)
        {
            throw new AppException("Token is invalid", HttpStatusCode.Unauthorized);
        }
    }

    private string GenerateToken(User user, TimeSpan expiry)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };
        var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");

        var key = new SymmetricSecurityKey(Convert.FromBase64String(jwtSecret!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.Add(expiry),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateAccessToken(User user) =>
        GenerateToken(user, TimeSpan.FromHours(ConstUtils.ExpireHours));

    public string GenerateRefreshToken(User user) =>
        GenerateToken(user, TimeSpan.FromDays(ConstUtils.ExpireRefresh));
}