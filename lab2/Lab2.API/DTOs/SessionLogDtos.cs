using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Lab2.DTOs;
public class CreateSessionLogDto
{
    [Required] public Guid SessionId { get; set; }
    public string? EventType { get; set; }
    public string? SeverityLevel { get; set; }
    public JsonElement? StateSnapshot { get; set; }
    public string? IpAddress { get; set; }
    public int? LatencyMs { get; set; }
}

public class UpdateSessionLogDto
{
    public string? EventType { get; set; }
    public string? SeverityLevel { get; set; }
    public JsonElement? StateSnapshot { get; set; }
    public string? IpAddress { get; set; }
    public int? LatencyMs { get; set; }
}

public class SessionLogDto
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public string? EventType { get; set; }
    public string? SeverityLevel { get; set; }
    public JsonElement? StateSnapshot { get; set; }
    public string? IpAddress { get; set; }
    public int? LatencyMs { get; set; }
    public DateTime CreatedAt { get; set; }
}
