using BoardService.Domain.Entities;
using BoardService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BoardService.Infrastructure.DbContext;

public class BoardDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public BoardDbContext(DbContextOptions<BoardDbContext> options) : base(options) { }

    public DbSet<Board> Boards { get; set; } = null!;
    
    public DbSet<BoardColumn> BoardColumns { get; set; } = null!;
    
    public DbSet<ColumnCard> Cards { get; set; }

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);
        model.Entity<Board>()
            .HasMany(b => b.Columns)
            .WithOne()
            .HasForeignKey(c => c.BoardId);

        model.Entity<BoardColumn>()
            .HasMany(c => c.Cards)
            .WithOne()
            .HasForeignKey(c => c.ColumnId);

        model.Entity<ColumnCard>()
            .HasKey(x => new { x.ColumnId, x.CardId });

    }
}