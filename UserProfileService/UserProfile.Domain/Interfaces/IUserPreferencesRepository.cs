using UserProfile.Domain.Entities;

namespace UserProfile.Domain.Interfaces;

public interface IUserPreferencesRepository
{
    Task<UserPreferences?> GetByUserProfileIdAsync(Guid profileId);
    Task AddAsync(UserPreferences preferences);
    Task UpdateAsync(UserPreferences preferences);
}