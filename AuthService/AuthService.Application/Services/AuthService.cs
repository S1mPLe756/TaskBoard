using System.Net;
using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Events;
using AuthService.Domain.Interfaces;
using AuthService.Domain.ValueObjects;
using AutoMapper;
using ExceptionService;

namespace AuthService.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;

    public AuthService(IUserRepository userRepository, ITokenService tokenService, IEventPublisher eventPublisher, IMapper mapper)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto)
    {
        var existing = await _userRepository.GetUserByEmailAsync(registerRequestDto.Email);
        if (existing != null)
            throw new AppException("User already exists", statusCode: HttpStatusCode.Conflict);

        var user = _mapper.Map<User>(registerRequestDto);
        
        user.Password = Password.FromPlainPassword(registerRequestDto.Password);
        user.Role = "User";
        await _userRepository.AddUserAsync(user);
        
        // Отправляем событие в Kafka
        var evt = new UserRegisteredEvent
        {
            UserId = user.Id,
            Email = user.Email,
        };

        await _eventPublisher.PublishUserRegisteredAsync(evt);

        
        return new RegisterResponseDto("Registration Successful", user.Id.ToString());
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email)
                   ?? throw new AppException("User not found", HttpStatusCode.NotFound);

        if (!user.Password.Verify(request.Password))
            throw new AppException("Invalid password");

        var tokens = await _tokenService.IssueTokensAsync(user);
        
        return new LoginResponseDto(tokens.AccessToken, tokens.RefreshToken);
    }

}