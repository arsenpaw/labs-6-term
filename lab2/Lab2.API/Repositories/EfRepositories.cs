using Lab2.Data;
using Lab2.Models;
using Microsoft.EntityFrameworkCore;
namespace Lab2.Repositories;
public class UserRepository(AppDbContext db) : BaseRepository<User, Guid>(db), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
        => await Set.FirstOrDefaultAsync(u => u.Email == email);
    public async Task<User?> GetByUsernameAsync(string username)
        => await Set.FirstOrDefaultAsync(u => u.Username == username);
    public async Task<bool> ExistsByEmailAsync(string email)
        => await Set.AnyAsync(u => u.Email == email);
    public async Task<bool> ExistsByUsernameAsync(string username)
        => await Set.AnyAsync(u => u.Username == username);
}
public class GameRepository(AppDbContext db) : BaseRepository<Game, Guid>(db), IGameRepository { }
public class GameSessionRepository(AppDbContext db) : BaseRepository<GameSession, Guid>(db), IGameSessionRepository
{
    public async Task<IEnumerable<GameSession>> GetByUserIdAsync(Guid userId)
        => await Set.AsNoTracking().Where(s => s.UserId == userId).ToListAsync();
    public async Task<IEnumerable<GameSession>> GetByGameIdAsync(Guid gameId)
        => await Set.AsNoTracking().Where(s => s.GameId == gameId).ToListAsync();
}
public class SessionLogRepository(AppDbContext db) : BaseRepository<SessionLog, Guid>(db), ISessionLogRepository
{
    public async Task<IEnumerable<SessionLog>> GetBySessionIdAsync(Guid sessionId)
        => await Set.AsNoTracking().Where(l => l.SessionId == sessionId).ToListAsync();
}