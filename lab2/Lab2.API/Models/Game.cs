using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lab2.Abstractions;
namespace Lab2.Models;
public class Game : Entity<Guid>, IAuditable
{
    [Required] public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Genre { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? Price { get; set; }
    public string? Publisher { get; set; }
    public float[]? Embedding { get; set; }
    public string[]? Tags { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<GameSession> GameSessions { get; set; } = [];
}