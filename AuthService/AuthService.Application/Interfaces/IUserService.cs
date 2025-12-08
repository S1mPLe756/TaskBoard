using AuthService.Application.DTOs.Responses;

namespace AuthService.Application.Interfaces;

public interface IUserService
{
    Task<List<UserResponse>> GetUsersByIdsAsync(List<Guid> ids);
    Task<UserResponse?> GetUsersByEmailAsync(string email);
}