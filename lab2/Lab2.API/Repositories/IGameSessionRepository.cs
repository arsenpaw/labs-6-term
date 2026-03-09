using Lab2.Abstractions;
using Lab2.Models;
namespace Lab2.Repositories;
public interface IGameSessionRepository : IRepository<GameSession, Guid>
{
    Task<IEnumerable<GameSession>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<GameSession>> GetByGameIdAsync(Guid gameId);
}