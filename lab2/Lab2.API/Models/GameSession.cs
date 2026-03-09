using Lab2.Abstractions;
namespace Lab2.Models;
public class GameSession : Entity<Guid>, IAuditable
{
    public Guid UserId { get; set; } 
    public Guid GameId { get; set; }
    public string? Status { get; set; }
    public int? CurrentLevel { get; set; }
    public long? ScoreCurrent { get; set; }
    public string? DeviceOs { get; set; }
    public string? ClientVersion { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public User User { get; set; }
    public Game Game { get; set; }
    public List<SessionLog> SessionLogs { get; set; } = [];
}