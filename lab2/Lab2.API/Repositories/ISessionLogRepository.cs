using Lab2.Abstractions;
using Lab2.Models;
namespace Lab2.Repositories;
public interface ISessionLogRepository : IRepository<SessionLog, Guid>
{
    Task<IEnumerable<SessionLog>> GetBySessionIdAsync(Guid sessionId);
}