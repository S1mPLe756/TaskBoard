using Microsoft.EntityFrameworkCore;
using UserProfile.Domain.Entities;
using UserProfile.Domain.Interfaces;
using UserProfile.Infrastructure.DbContext;

namespace UserProfile.Infrastructure.Repositories;

public class UserPreferencesRepository : IUserPreferencesRepository
{
    private readonly UserProfileDbContext _context;

    public UserPreferencesRepository(UserProfileDbContext context)
    {
        _context = context;
    }

    public async Task<UserPreferences?> GetByUserProfileIdAsync(Guid profileId)
        => await _context.UserPreferences.FirstOrDefaultAsync(p => p.UserProfileId == profileId);

    public async Task AddAsync(UserPreferences preferences)
    {
        _context.UserPreferences.Add(preferences);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserPreferences preferences)
    {
        _context.UserPreferences.Update(preferences);
        await _context.SaveChangesAsync();
    }
}