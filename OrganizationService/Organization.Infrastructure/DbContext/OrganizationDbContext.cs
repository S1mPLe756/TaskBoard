using Organization.Domain.Entities;

namespace Organization.Infrastructure.DbContext;

using Microsoft.EntityFrameworkCore;

public class OrganizationDbContext : DbContext
{
    public DbSet<Workspace> Workspaces { get; set; } = null!;
    
    public DbSet<WorkspaceMember> WorkspaceMembers { get; set; } = null!;
    
    public DbSet<Invitation> Invitations { get; set; } = null!;

    
    public OrganizationDbContext(DbContextOptions<OrganizationDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WorkspaceMember>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.UserId).IsRequired();
            
        });

        modelBuilder.Entity<WorkspaceMember>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.UserId).IsRequired();
            builder.Property(u => u.WorkspaceId).IsRequired();
        });
        
        modelBuilder.Entity<Workspace>()
            .HasMany(p => p.Members)
            .WithOne(p => p.Workspace)
            .HasForeignKey(p=>p.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
 
    }


}