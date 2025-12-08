using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _dbContext;

    public UserRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserByEmailAsync(string email) =>
        await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetUserByIdAsync(Guid id) =>
        await _dbContext.Users.FindAsync(id);

    public async Task AddUserAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsersByIdsAsync(List<Guid> ids) => await _dbContext.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
}