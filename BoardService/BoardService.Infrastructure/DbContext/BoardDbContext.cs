using BoardService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoardService.Infrastructure.DbContext;

public class BoardDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public BoardDbContext(DbContextOptions<BoardDbContext> options) : base(options) { }

    public DbSet<Board> Boards { get; set; } = null!;
    
    public DbSet<BoardColumn> BoardColumns { get; set; } = null!;
    
    public DbSet<CardPosition> CardPositions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);
        
        model.Entity<Board>()
            .HasMany(b => b.Columns)
            .WithOne(column => column.Board)
            .HasForeignKey(column => column.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        model.Entity<CardPosition>()
            .HasOne(cp => cp.Column)
            .WithMany(c => c.Cards)
            .HasForeignKey(cp => cp.ColumnId)
            .OnDelete(DeleteBehavior.Cascade);

        model.Entity<CardPosition>()
            .HasKey(cp => cp.Id);

    }
}