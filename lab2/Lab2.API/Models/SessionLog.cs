using System.Text.Json;
using Lab2.Abstractions;
namespace Lab2.Models;
public class SessionLog : Entity<Guid>, IAuditable
{
    public Guid SessionId { get; set; }
    public string? EventType { get; set; }
    public string? SeverityLevel { get; set; }
    public JsonDocument? StateSnapshot { get; set; }
    public string? IpAddress { get; set; }
    public int? LatencyMs { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public GameSession Session { get; set; }
}