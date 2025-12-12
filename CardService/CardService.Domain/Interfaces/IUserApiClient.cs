using CardService.Domain.DTOs;

namespace CardService.Domain.Interfaces;

public interface IUserApiClient
{
    public Task<List<UserResponse>> GetUsers(List<Guid> ids);
    
    public Task<UserResponse?> GetUserByEmail(string email);

}