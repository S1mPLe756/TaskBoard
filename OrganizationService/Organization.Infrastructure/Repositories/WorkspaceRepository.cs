using Microsoft.EntityFrameworkCore;
using Organization.Domain.Entities;
using Organization.Domain.Interfaces;
using Organization.Infrastructure.DbContext;

namespace Organization.Infrastructure.Repositories;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly OrganizationDbContext _dbContext;

    public WorkspaceRepository(OrganizationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(Workspace workspace)
    {
        await _dbContext.Workspaces.AddAsync(workspace);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Workspace?> GetByIdAsync(Guid workspaceId) => await _dbContext.Workspaces.Include(x=>x.Members).FirstOrDefaultAsync(w=> w.Id == workspaceId);

    public async Task<List<Workspace>> GetByUserIdAsync(Guid userId) => await _dbContext.Workspaces.Where(w=> w.OwnerId == userId || w.Members.Any(m => m.UserId == userId)).ToListAsync();

    public async Task UpdateAsync(Workspace workspace)
    {
        _dbContext.Workspaces.Update(workspace);
        await _dbContext.SaveChangesAsync();
    }
}