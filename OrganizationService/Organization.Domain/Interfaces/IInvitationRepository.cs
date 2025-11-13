using Organization.Domain.Entities;

namespace Organization.Domain.Interfaces;

public interface IInvitationRepository
{
    Task<Invitation?> GetByIdAsync(Guid id);
    Task<List<Invitation>> GetPendingByEmailAsync(string email);
    Task AddAsync(Invitation invitation);
    Task UpdateAsync(Invitation invitation);
}