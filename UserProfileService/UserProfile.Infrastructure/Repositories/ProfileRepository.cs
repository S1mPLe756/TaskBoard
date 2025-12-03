using Microsoft.EntityFrameworkCore;
using UserProfile.Domain.Interfaces;
using UserProfile.Domain.Entities;
using UserProfile.Infrastructure.DbContext;

namespace UserProfile.Infrastructure.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly UserProfileDbContext _context;

    public ProfileRepository(UserProfileDbContext context)
    {
        _context = context;
    }
    
    public async Task<Profile?> GetByUserIdAsync(Guid userId)
        => await _context.UserProfiles.Include(p => p.Preferences)
            .FirstOrDefaultAsync(x => x.UserId == userId);


    public async Task AddAsync(Profile profile)
    {
        _context.UserProfiles.Add(profile);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Profile profile)
    {
        _context.UserProfiles.Update(profile);
        await _context.SaveChangesAsync();
    }
}