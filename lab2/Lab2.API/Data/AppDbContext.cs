using System.Text.Json;
using Lab2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Lab2.Data;
public class JsonDocumentConverter : ValueConverter<JsonDocument?, string?>
{
    public JsonDocumentConverter()
        : base(
            v => v == null ? null : v.RootElement.GetRawText(),
            v => v == null ? null : JsonDocument.Parse(v))
    {
    }
}
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<GameSession> GameSessions => Set<GameSession>();
    public DbSet<SessionLog> SessionLogs => Set<SessionLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SessionLog>()
            .Property(e => e.StateSnapshot)
            .HasConversion(new JsonDocumentConverter());
    }
}
