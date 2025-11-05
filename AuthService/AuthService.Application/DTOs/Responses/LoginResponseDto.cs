namespace AuthService.Application.DTOs;

public record LoginResponseDto(string AccessToken, string RefreshToken);