using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lab2.DTOs;
using Lab2.Models;
using Lab2.Repositories;
using Microsoft.IdentityModel.Tokens;
namespace Lab2.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    public UserService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            CreatedAt = u.CreatedAt
        });
    }
    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null) return null;
        return new UserDto { Id = user.Id, Username = user.Username, Email = user.Email, CreatedAt = user.CreatedAt };
    }
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        if (await _userRepository.ExistsByEmailAsync(dto.Email))
            throw new InvalidOperationException("Email already in use.");
        if (await _userRepository.ExistsByUsernameAsync(dto.Username))
            throw new InvalidOperationException("Username already taken.");
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        var created = await _userRepository.CreateAsync(user);
        return new AuthResponseDto
        {
            Token = GenerateJwtToken(created),
            Username = created.Username,
            Email = created.Email
        };
    }
    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email)
                   ?? throw new UnauthorizedAccessException("Invalid credentials.");
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials.");
        return new AuthResponseDto
        {
            Token = GenerateJwtToken(user),
            Username = user.Username,
            Email = user.Email
        };
    }
    public async Task<UserDto?> UpdateUserAsync(Guid id, RegisterDto dto)
    {
        var updated = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        var result = await _userRepository.UpdateAsync(id, updated);
        if (result is null) return null;
        return new UserDto { Id = result.Id, Username = result.Username, Email = result.Email, CreatedAt = result.CreatedAt };
    }
    public Task<bool> DeleteUserAsync(Guid id) => _userRepository.DeleteAsync(id);
    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"] ?? "60")),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}