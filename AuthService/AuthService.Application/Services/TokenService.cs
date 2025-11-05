using System.Net;
using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using ExceptionService;

namespace AuthService.Application.Services;

public class TokenService : ITokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenProvider _jwtTokenProvider;

    public TokenService(IRefreshTokenRepository refreshTokenRepository, IJwtTokenProvider tokenProvider)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenProvider = tokenProvider;
    }
    
    public async Task<TokenPairDto> IssueTokensAsync(User user)
    {
        var access = _jwtTokenProvider.GenerateAccessToken(user);
        var refreshToken = _jwtTokenProvider.GenerateRefreshToken(user);
        
        var refresh = new RefreshToken
        {
            User = user,
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(30),
            IsRevoked = false
        };

        await _refreshTokenRepository.RevokeAllByUserIdAsync(user.Id);
        await _refreshTokenRepository.SaveTokenAsync(refresh);

        return new TokenPairDto(access, refresh.Token);
    }

    
    public async Task<TokenPairDto> RefreshToken(string refreshToken)
    {
        RefreshToken? refresh = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (refresh == null || refresh.IsRevoked || refresh.IsExpired)
        {
            throw new AppException("Token is expired or invalid", HttpStatusCode.Unauthorized);
        }
        
        return await IssueTokensAsync(refresh.User);
    }

    public Task Validate(string token)
    {
        _jwtTokenProvider.ValidateToken(token);
        
        return Task.CompletedTask;
    }
}