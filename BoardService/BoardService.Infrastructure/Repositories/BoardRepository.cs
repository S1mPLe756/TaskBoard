using BoardService.Domain.Entities;
using BoardService.Domain.Interfaces;
using BoardService.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace BoardService.Infrastructure.Repositories;

public class BoardRepository : IBoardRepository
{
    private readonly BoardDbContext _dbContext;

    public BoardRepository(BoardDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Board?> GetBoardByIdAsync(Guid id) =>
        await _dbContext.Boards.Include(b => b.Columns).ThenInclude(x=>x.Cards).FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddBoardAsync(Board board)
    {
        await _dbContext.Boards.AddAsync(board);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateBoardAsync(Board board)
    {
        _dbContext.Boards.Update(board);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Board>> GetBoardsByWorkspaceAsync(Guid workspaceId) =>
        await _dbContext.Boards.Include(b => b.Columns).Where(x => x.WorkspaceId == workspaceId).ToListAsync();
}