using Lab2.Abstractions;
using Lab2.DTOs;
using Lab2.Models;

namespace Lab2.Services;

public class GameSessionService(IUnitOfWork uow) : IGameSessionService
{
    public async Task<IEnumerable<GameSessionDto>> GetAllAsync()
        => (await uow.GameSessions.GetAllAsync()).Select(ToDto);

    public async Task<GameSessionDto?> GetByIdAsync(Guid id)
    {
        var s = await uow.GameSessions.GetByIdAsync((id));
        return s is null ? null : ToDto(s);
    }

    public async Task<IEnumerable<GameSessionDto>> GetByUserIdAsync(Guid userId)
        => (await uow.GameSessions.GetByUserIdAsync((userId))).Select(ToDto);

    public async Task<IEnumerable<GameSessionDto>> GetByGameIdAsync(Guid gameId)
        => (await uow.GameSessions.GetByGameIdAsync((gameId))).Select(ToDto);

    public async Task<GameSessionDto> CreateAsync(CreateGameSessionDto dto)
    {
        if (!await uow.Users.ExistsByIdAsync((dto.UserId)))
            throw new KeyNotFoundException($"User '{dto.UserId}' not found.");
        if (!await uow.Games.ExistsByIdAsync((dto.GameId)))
            throw new KeyNotFoundException($"Game '{dto.GameId}' not found.");

        var session = new GameSession
        {
            Id = Guid.NewGuid(),
            UserId = (dto.UserId),
            GameId = (dto.GameId),
            Status = dto.Status,
            CurrentLevel = dto.CurrentLevel,
            ScoreCurrent = dto.ScoreCurrent,
            DeviceOs = dto.DeviceOs,
            ClientVersion = dto.ClientVersion
        };
        await uow.GameSessions.AddAsync(session);
        await uow.SaveChangesAsync();
        return ToDto(session);
    }

    public async Task<GameSessionDto?> UpdateAsync(Guid id, UpdateGameSessionDto dto)
    {
        var existing = await uow.GameSessions.GetByIdAsync(id);
        if (existing is null) return null;

        existing.Status = dto.Status;
        existing.CurrentLevel = dto.CurrentLevel;
        existing.ScoreCurrent = dto.ScoreCurrent;
        existing.DeviceOs = dto.DeviceOs;
        existing.ClientVersion = dto.ClientVersion;
        existing.EndedAt = dto.EndedAt;

        var result = await uow.GameSessions.UpdateAsync(id, existing);
        if (result is null) return null;
        await uow.SaveChangesAsync();
        return ToDto(result);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await uow.GameSessions.DeleteAsync((id));
        if (deleted) await uow.SaveChangesAsync();
        return deleted;
    }

    private static GameSessionDto ToDto(GameSession s) => new()
    {
        Id = s.Id,
        UserId = s.UserId,
        GameId = s.GameId,
        Status = s.Status,
        CurrentLevel = s.CurrentLevel,
        ScoreCurrent = s.ScoreCurrent,
        DeviceOs = s.DeviceOs,
        ClientVersion = s.ClientVersion,
        StartedAt = s.StartedAt,
        EndedAt = s.EndedAt
    };
}
