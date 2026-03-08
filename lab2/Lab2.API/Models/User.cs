using System.ComponentModel.DataAnnotations;
using Lab2.Abstractions;
namespace Lab2.Models;
public class User : Entity<Guid>, IAuditable
{
    [Required] public string Username { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    [StringLength(2)] public string? RegionCode { get; set; }
    public string? AvatarUrl { get; set; }
    public bool IsVerified { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<GameSession> GameSessions { get; set; } = [];
}