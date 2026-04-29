using Microsoft.EntityFrameworkCore;
using PakDzal_Games_API.Models;

namespace PakDzal_Games_API.Data;

public class GameClubDbContext : DbContext
{
    public GameClubDbContext(DbContextOptions<GameClubDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.City).HasDefaultValue("Курск");
            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("CURRENT_DATE");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId);
            entity.Property(e => e.Available).HasDefaultValue(true);
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Sessions)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Game)
                  .WithMany(g => g.Sessions)
                  .HasForeignKey(e => e.GameId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}