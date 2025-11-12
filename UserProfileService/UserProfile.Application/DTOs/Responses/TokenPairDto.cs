namespace AuthService.Application.DTOs;

public record TokenPairDto(string AccessToken, string RefreshToken);