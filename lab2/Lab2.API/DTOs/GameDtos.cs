using System.ComponentModel.DataAnnotations;

namespace Lab2.DTOs;

// ─── Game ─────────────────────────────────────────────────────────────────────

public class CreateGameDto
{
    [Required] public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Genre { get; set; }
    [Range(0, double.MaxValue)] public decimal? Price { get; set; }
    public string? Publisher { get; set; }
    /// <summary>Vector(768) — exactly 768 floats when provided.</summary>
    public float[]? Embedding { get; set; }
    public string[]? Tags { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateGameDto
{
    [Required] public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Genre { get; set; }
    [Range(0, double.MaxValue)] public decimal? Price { get; set; }
    public string? Publisher { get; set; }
    public float[]? Embedding { get; set; }
    public string[]? Tags { get; set; }
    public bool IsActive { get; set; } = true;
}

public class GameDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Genre { get; set; }
    public decimal? Price { get; set; }
    public string? Publisher { get; set; }
    public float[]? Embedding { get; set; }
    public string[]? Tags { get; set; }
    public bool IsActive { get; set; }
}

