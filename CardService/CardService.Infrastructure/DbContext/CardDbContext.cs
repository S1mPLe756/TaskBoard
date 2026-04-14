using CardService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CardService.Infrastructure.DbContext;

public class CardDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public CardDbContext(DbContextOptions<CardDbContext> options) : base(options) { }

    public DbSet<Card> Cards { get; set; } = null!;
    public DbSet<CardLabel> CardLabels { get; set; } = null!;
    public DbSet<CardChecklist> CardChecklists { get; set; } = null!;
    public DbSet<CardChecklistItem> CardChecklistItems { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);
        
        model.Entity<Card>()
            .HasKey(x => x.Id);

        model.Entity<Card>()
            .HasMany(x=>x.Labels)
            .WithMany(x=>x.Cards)
            .UsingEntity(j => j.ToTable("CardCardLabels"));
        model.Entity<Card>()
            .HasOne(x=>x.Checklist)
            .WithOne(x=>x.Card)
            .OnDelete(DeleteBehavior.Cascade);
        
        model.Entity<Card>()
            .HasMany(x=>x.Attachments)
            .WithOne(x=>x.Card)
            .HasForeignKey(x=>x.CardId)
            .OnDelete(DeleteBehavior.Cascade);
        
        model.Entity<CardChecklist>()
            .HasMany(c=>c.Items)
            .WithOne(x=>x.Checklist)
            .HasForeignKey(x=>x.ChecklistId)
            .OnDelete(DeleteBehavior.Cascade);
        
        model.Entity<Comment>()
            .HasOne(x => x.Card)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.CardId)
            .OnDelete(DeleteBehavior.Cascade);
        
        model.Entity<Comment>()
            .Property(c => c.AnsweredCommentId)
            .IsRequired(false);
        
        model.Entity<Comment>()
            .HasMany(x => x.AnswerComments)
            .WithOne(x => x.AnsweredComment)
            .HasForeignKey(x => x.AnsweredCommentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}