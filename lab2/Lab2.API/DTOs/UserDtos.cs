using System.ComponentModel.DataAnnotations;

namespace Lab2.DTOs;

// ─── User ────────────────────────────────────────────────────────────────────

public class CreateUserDto
{
    [Required] public string Username { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
    [StringLength(2)] public string? RegionCode { get; set; }
    public string? AvatarUrl { get; set; }
}

public class UpdateUserDto
{
    [Required] public string Username { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [MinLength(6)] public string? Password { get; set; }
    [StringLength(2)] public string? RegionCode { get; set; }
    public string? AvatarUrl { get; set; }
    public bool IsVerified { get; set; }
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? RegionCode { get; set; }
    public string? AvatarUrl { get; set; }
    public bool IsVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}