using System.Text.Json;
using Lab2.Abstractions;
using Lab2.DTOs;
using Lab2.Models;
namespace Lab2.Services;
public class SessionLogService(IUnitOfWork uow) : ISessionLogService
{
    public async Task<IEnumerable<SessionLogDto>> GetAllAsync()
        => (await uow.SessionLogs.GetAllAsync()).Select(ToDto);
    public async Task<SessionLogDto?> GetByIdAsync(Guid id)
    {
        var log = await uow.SessionLogs.GetByIdAsync(id);
        return log is null ? null : ToDto(log);
    }
    public async Task<IEnumerable<SessionLogDto>> GetBySessionIdAsync(Guid sessionId)
        => (await uow.SessionLogs.GetBySessionIdAsync(sessionId)).Select(ToDto);
    public async Task<SessionLogDto> CreateAsync(CreateSessionLogDto dto)
    {
        if (!await uow.GameSessions.ExistsByIdAsync(dto.SessionId))
            throw new KeyNotFoundException($"GameSession '{dto.SessionId}' not found.");
        var log = new SessionLog
        {
            Id = Guid.NewGuid(),
            SessionId =dto.SessionId,
            EventType = dto.EventType,
            SeverityLevel = dto.SeverityLevel,
            StateSnapshot = dto.StateSnapshot.HasValue
                ? JsonDocument.Parse(dto.StateSnapshot.Value.GetRawText())
                : null,
            IpAddress = dto.IpAddress,
            LatencyMs = dto.LatencyMs
        };
        await uow.SessionLogs.AddAsync(log);
        await uow.SaveChangesAsync();
        return ToDto(log);
    }
    public async Task<SessionLogDto?> UpdateAsync(Guid id, UpdateSessionLogDto dto)
    {
        var existing = await uow.SessionLogs.GetByIdAsync(id);
        if (existing is null) return null;

        existing.EventType = dto.EventType;
        existing.SeverityLevel = dto.SeverityLevel;
        existing.StateSnapshot = dto.StateSnapshot.HasValue
            ? JsonDocument.Parse(dto.StateSnapshot.Value.GetRawText())
            : null;
        existing.IpAddress = dto.IpAddress;
        existing.LatencyMs = dto.LatencyMs;

        var result = await uow.SessionLogs.UpdateAsync(id, existing);
        if (result is null) return null;
        await uow.SaveChangesAsync();
        return ToDto(result);
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await uow.SessionLogs.DeleteAsync((id));
        if (deleted) await uow.SaveChangesAsync();
        return deleted;
    }
    private static SessionLogDto ToDto(SessionLog l) => new()
    {
        Id = l.Id,
        SessionId = l.SessionId,
        EventType = l.EventType,
        SeverityLevel = l.SeverityLevel,
        StateSnapshot = l.StateSnapshot is not null
            ? JsonSerializer.Deserialize<JsonElement>(l.StateSnapshot.RootElement.GetRawText())
            : null,
        IpAddress = l.IpAddress,
        LatencyMs = l.LatencyMs,
        CreatedAt = l.CreatedAt
    };
}