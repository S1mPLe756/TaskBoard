using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Requestes;
using AuthService.Application.DTOs.Responses;
using AuthService.Domain.Entities;

namespace AuthService.Application.Interfaces;

public interface ITokenService
{
    Task<TokenPairDto> IssueTokensAsync(User user);

    Task<TokenPairDto> RefreshToken(RefreshTokenRequest refreshToken);
    Task Validate(string token);
}