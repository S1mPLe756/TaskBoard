namespace AuthService.Application.DTOs.Responses;

public record TokenPairDto(string AccessToken, string RefreshToken);