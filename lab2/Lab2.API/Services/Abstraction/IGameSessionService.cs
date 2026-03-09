using Lab2.DTOs;

namespace Lab2.Services;

public interface IGameSessionService
{
    Task<IEnumerable<GameSessionDto>> GetAllAsync();
    Task<GameSessionDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<GameSessionDto>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<GameSessionDto>> GetByGameIdAsync(Guid gameId);
    Task<GameSessionDto> CreateAsync(CreateGameSessionDto dto);
    Task<GameSessionDto?> UpdateAsync(Guid id, UpdateGameSessionDto dto);
    Task<bool> DeleteAsync(Guid id);
}

