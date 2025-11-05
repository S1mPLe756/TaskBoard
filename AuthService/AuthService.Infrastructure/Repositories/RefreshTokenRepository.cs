using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AuthDbContext _dbContext;

    public RefreshTokenRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<RefreshToken?> GetByTokenAsync(string token) =>
    await _dbContext.RefreshTokens.Include(r=>r.User).FirstOrDefaultAsync(u => u.Token == token);


    public async Task SaveTokenAsync(RefreshToken token)
    {
        await _dbContext.RefreshTokens.AddAsync(token);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RevokeAllByUserIdAsync(Guid userId)
    {
        var tokens = await _dbContext.RefreshTokens.Where(token => token.UserId == userId).ToListAsync();

        foreach (var refreshToken in tokens)
        {
            refreshToken.IsRevoked = true;
        }
        
        await _dbContext.SaveChangesAsync();
    }
}