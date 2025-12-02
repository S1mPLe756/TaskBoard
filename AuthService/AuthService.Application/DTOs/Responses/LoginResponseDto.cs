namespace AuthService.Application.DTOs.Responses;

public record LoginResponseDto(string AccessToken, string RefreshToken);