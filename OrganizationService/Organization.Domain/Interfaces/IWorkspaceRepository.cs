using Organization.Domain.Entities;

namespace Organization.Domain.Interfaces;

public interface IWorkspaceRepository
{
    Task<Workspace?> GetByIdAsync(Guid workspaceId);
    Task<List<Workspace>> GetByUserIdAsync(Guid userId);
    Task AddAsync(Workspace workspace);
    Task UpdateAsync(Workspace workspace);
    Task DeleteAsync(Workspace workspace);
}