using AuthService.Application.DTOs;

namespace AuthService.Application.Interfaces;

public interface IAuthService
{
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto);

    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);

}