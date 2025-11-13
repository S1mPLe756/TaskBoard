using Microsoft.EntityFrameworkCore;
using Organization.Domain.Entities;
using Organization.Domain.Interfaces;
using Organization.Infrastructure.DbContext;

namespace Organization.Infrastructure.Repositories;

public class InvitationRepository : IInvitationRepository
{
    private readonly OrganizationDbContext _dbContext;

    public InvitationRepository(OrganizationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Invitation?> GetByIdAsync(Guid id) => await _dbContext.Invitations.FirstOrDefaultAsync(i=>i.Id == id);

    public async Task<List<Invitation>> GetPendingByEmailAsync(string email) => await _dbContext.Invitations.Where(i => i.Email == email).ToListAsync();

    public async Task AddAsync(Invitation invitation)
    {
        await _dbContext.Invitations.AddAsync(invitation);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Invitation invitation)
    {
        _dbContext.Invitations.Update(invitation);
        await _dbContext.SaveChangesAsync();
    }
}