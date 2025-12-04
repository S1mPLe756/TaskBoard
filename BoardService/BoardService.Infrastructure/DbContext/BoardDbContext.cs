using BoardService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoardService.Infrastructure.DbContext;

public class BoardDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public BoardDbContext(DbContextOptions<BoardDbContext> options) : base(options) { }

    public DbSet<Board> Boards { get; set; } = null!;
    
    public DbSet<BoardColumn> BoardColumns { get; set; } = null!;
    
    public DbSet<Card> Cards { get; set; }

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);
        
        model.Entity<Board>()
            .HasMany(b => b.Columns)
            .WithOne(column => column.Board)
            .HasForeignKey(column => column.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        model.Entity<BoardColumn>()
            .HasMany(c => c.Cards)
            .WithOne(card => card.Column)
            .HasForeignKey(card => card.ColumnId)
            .OnDelete(DeleteBehavior.Cascade);

        model.Entity<Card>()
            .HasKey(x => x.Id);

    }
}