using Lab2.DTOs;
namespace Lab2.Services;
public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<UserDto?> UpdateUserAsync(Guid id, RegisterDto dto);
    Task<bool> DeleteUserAsync(Guid id);
}