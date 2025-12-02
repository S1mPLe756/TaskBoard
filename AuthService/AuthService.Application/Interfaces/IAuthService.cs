using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Responses;

namespace AuthService.Application.Interfaces;

public interface IAuthService
{
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto);

    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);

}