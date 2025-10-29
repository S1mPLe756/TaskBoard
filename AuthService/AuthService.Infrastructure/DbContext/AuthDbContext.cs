namespace AuthService.Infrastructure.DbContext;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email).IsRequired();

            builder.OwnsOne(u => u.Password, pw =>
            {
                pw.Property(p => p.Hash)
                    .HasColumnName("PasswordHash")
                    .IsRequired();
            });
        });
    }
}