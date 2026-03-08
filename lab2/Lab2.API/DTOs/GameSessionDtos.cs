using System.ComponentModel.DataAnnotations;

namespace Lab2.DTOs;

// ─── GameSession ──────────────────────────────────────────────────────────────

public class CreateGameSessionDto
{
    [Required] public Guid UserId { get; set; }
    [Required] public Guid GameId { get; set; }
    public string? Status { get; set; }
    public int? CurrentLevel { get; set; }
    public long? ScoreCurrent { get; set; }
    public string? DeviceOs { get; set; }
    public string? ClientVersion { get; set; }
}

public class UpdateGameSessionDto
{
    public string? Status { get; set; }
    public int? CurrentLevel { get; set; }
    public long? ScoreCurrent { get; set; }
    public string? DeviceOs { get; set; }
    public string? ClientVersion { get; set; }
    public DateTime? EndedAt { get; set; }
}

public class GameSessionDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
    public string? Status { get; set; }
    public int? CurrentLevel { get; set; }
    public long? ScoreCurrent { get; set; }
    public string? DeviceOs { get; set; }
    public string? ClientVersion { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
}

