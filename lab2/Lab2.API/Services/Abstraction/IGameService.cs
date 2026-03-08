using Lab2.DTOs;

namespace Lab2.Services;

public interface IGameService
{
    Task<IEnumerable<GameDto>> GetAllGamesAsync();
    Task<GameDto?> GetGameByIdAsync(Guid id);
    Task<GameDto> CreateGameAsync(CreateGameDto dto);
    Task<GameDto?> UpdateGameAsync(Guid id, UpdateGameDto dto);
    Task<bool> DeleteGameAsync(Guid id);
}

