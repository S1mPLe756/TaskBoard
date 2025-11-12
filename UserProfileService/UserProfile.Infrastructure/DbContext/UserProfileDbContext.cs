namespace UserProfile.Infrastructure.DbContext;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;


public class UserProfileDbContext : DbContext
{
    public DbSet<Profile> UserProfiles { get; set; } = null!;
    public DbSet<UserPreferences> UserPreferences { get; set; } = null!;

    public UserProfileDbContext(DbContextOptions<UserProfileDbContext> options)
        : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Profile>(builder =>
        {
            builder.HasKey(u => u.UserId);
            builder.Property(u => u.FullName).IsRequired();
            
        });
        
        modelBuilder.Entity<Profile>()
            .HasOne(p => p.Preferences)
            .WithOne(p => p.Profile)
            .HasForeignKey<UserPreferences>(p => p.UserProfileId)
            .OnDelete(DeleteBehavior.Cascade);
 
    }
}