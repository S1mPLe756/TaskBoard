using Organization.Domain.DTOs;

namespace Organization.Domain.Interfaces;

public interface IUserApiClient
{
    public Task<List<UserResponse>> GetUsers(List<Guid> ids);
    
    public Task<UserResponse?> GetUserByEmail(string email);

}