using CardService.Domain.DTOs;
using Refit;

namespace CardService.Infrastructure.ExternalAPI;

public interface IUserApiRefitClient
{
    [Post("/api/v1/User/bulk")]
    public Task<List<UserResponse>> GetUsers(List<Guid> ids);
    
    [Get("/api/v1/User/email/{email}")]
    public Task<UserResponse?> GetUserByEmail(string email);

}