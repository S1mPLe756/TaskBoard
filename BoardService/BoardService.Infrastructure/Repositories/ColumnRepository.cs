using BoardService.Domain.Entities;
using BoardService.Domain.Interfaces;
using BoardService.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace BoardService.Infrastructure.Repositories;

public class ColumnRepository : IColumnRepository
{
    private BoardDbContext _context;

    public ColumnRepository(BoardDbContext context)
    {
        _context = context;
    }
    
    public async Task<BoardColumn?> GetColumnByIdAsync(Guid id) => await _context.BoardColumns.FindAsync(id);

    public async Task AddColumnAsync(BoardColumn board)
    {
        await _context.BoardColumns.AddAsync(board);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateColumnAsync(BoardColumn board)
    {
        _context.BoardColumns.Update(board);
        await _context.SaveChangesAsync();
    }

    public async Task<List<BoardColumn>> GetColumnByBoardIdAsync(Guid boardId) =>
        await _context.BoardColumns.Where(bc => bc.BoardId == boardId).ToListAsync();
}