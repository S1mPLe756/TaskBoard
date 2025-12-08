using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(Guid id); 
    Task AddUserAsync(User user);
    Task<List<User>> GetUsersByIdsAsync(List<Guid> ids);
}