using AuthService.Application.DTOs.Responses;
using AuthService.Application.Interfaces;
using AuthService.Domain.Interfaces;
using AutoMapper;

namespace AuthService.Application.Services;

public class UserService(IUserRepository repository, IMapper mapper) : IUserService
{
    public async Task<List<UserResponse>> GetUsersByIdsAsync(List<Guid> ids)
    {
        var users = await repository.GetUsersByIdsAsync(ids);
        
        return users.Select(mapper.Map<UserResponse>).ToList();
    }

    public async Task<UserResponse?> GetUsersByEmailAsync(string email)
    {
        var user = await repository.GetUserByEmailAsync(email);
        
        return user is not null ? mapper.Map<UserResponse>(user) : null;
    }
}