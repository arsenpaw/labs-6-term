using Lab2.Repositories;

namespace Lab2.Abstractions;

public interface IUnitOfWork : IAsyncDisposable
{
    IUserRepository Users { get; }
    IGameRepository Games { get; }
    IGameSessionRepository GameSessions { get; }
    ISessionLogRepository SessionLogs { get; }

    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitAsync(CancellationToken ct = default);
    Task RollbackAsync(CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

