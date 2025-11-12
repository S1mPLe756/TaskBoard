using UserProfile.Domain.Entities;

namespace UserProfile.Domain.Interfaces;

public interface IProfileRepository
{
    Task<Profile?> GetByUserIdAsync(Guid userId);
    Task AddAsync(Profile profile);

}