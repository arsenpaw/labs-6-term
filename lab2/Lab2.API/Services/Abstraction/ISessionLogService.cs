using Lab2.DTOs;

namespace Lab2.Services;

public interface ISessionLogService
{
    Task<IEnumerable<SessionLogDto>> GetAllAsync();
    Task<SessionLogDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<SessionLogDto>> GetBySessionIdAsync(Guid sessionId);
    Task<SessionLogDto> CreateAsync(CreateSessionLogDto dto);
    Task<SessionLogDto?> UpdateAsync(Guid id, UpdateSessionLogDto dto);
    Task<bool> DeleteAsync(Guid id);
}

