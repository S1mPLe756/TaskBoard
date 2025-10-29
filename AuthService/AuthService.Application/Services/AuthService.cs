using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Domain.ValueObjects;
using AutoMapper;

namespace AuthService.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;

    public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
    }

    public async Task<string> RegisterAsync(RegisterRequestDto registerRequestDto)
    {
        var existing = await _userRepository.GetUserByEmailAsync(registerRequestDto.Email);
        if (existing != null)
            throw new Exception("User already exists");

        var user = _mapper.Map<User>(registerRequestDto);
        
        user.Password = Password.FromPlainPassword(registerRequestDto.Password);
        user.Role = "User";
        await _userRepository.AddUserAsync(user);
        return _jwtTokenGenerator.GenerateToken(user);
    }

    // TODO Создать свои Exception
    public async Task<string> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email)
                   ?? throw new Exception("User not found");

        if (!user.Password.Verify(request.Password))
            throw new Exception("Invalid password");

        return _jwtTokenGenerator.GenerateToken(user);
    }

}