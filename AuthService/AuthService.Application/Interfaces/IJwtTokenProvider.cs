using AuthService.Domain.Entities;

namespace AuthService.Application.Interfaces;

public interface IJwtTokenProvider
{
    string GenerateAccessToken(User user);
    bool ValidateToken(string token);
    string GenerateRefreshToken(User user);

}