using Lab2.Abstractions;
using Lab2.DTOs;
using Lab2.Models;
namespace Lab2.Services;
public class UserService(IUnitOfWork uow) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetAllAsync()
        => (await uow.Users.GetAllAsync()).Select(ToDto);
    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await uow.Users.GetByIdAsync((id));
        return user is null ? null : ToDto(user);
    }
    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        if (await uow.Users.ExistsByEmailAsync(dto.Email))
            throw new InvalidOperationException("Email already in use.");
        if (await uow.Users.ExistsByUsernameAsync(dto.Username))
            throw new InvalidOperationException("Username already taken.");
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            RegionCode = dto.RegionCode,
            AvatarUrl = dto.AvatarUrl
        };
        await uow.Users.AddAsync(user);
        await uow.SaveChangesAsync();
        return ToDto(user);
    }
    public async Task<UserDto?> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var existing = await uow.Users.GetByIdAsync(id);
        if (existing is null) return null;

        existing.Username = dto.Username;
        existing.Email = dto.Email;
        if (!string.IsNullOrWhiteSpace(dto.Password))
            existing.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        existing.RegionCode = dto.RegionCode;
        existing.AvatarUrl = dto.AvatarUrl;
        existing.IsVerified = dto.IsVerified;

        await uow.Users.UpdateAsync(id, existing);
        await uow.SaveChangesAsync();
        return ToDto(existing);
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await uow.Users.DeleteAsync((id));
        if (deleted) await uow.SaveChangesAsync();
        return deleted;
    }
    private static UserDto ToDto(User u) => new()
    {
        Id = u.Id,
        Username = u.Username,
        Email = u.Email,
        RegionCode = u.RegionCode,
        AvatarUrl = u.AvatarUrl,
        IsVerified = u.IsVerified,
        CreatedAt = u.CreatedAt,
        UpdatedAt = u.UpdatedAt
    };
}