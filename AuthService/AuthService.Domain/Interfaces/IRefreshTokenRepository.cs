using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task SaveTokenAsync(RefreshToken token);
    Task RevokeAllByUserIdAsync(Guid userId);
}