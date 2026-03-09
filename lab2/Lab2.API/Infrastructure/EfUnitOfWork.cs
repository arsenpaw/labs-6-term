using Lab2.Abstractions;
using Lab2.Data;
using Lab2.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Lab2.Infrastructure;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    private IDbContextTransaction? _tx;

    public IUserRepository Users { get; }
    public IGameRepository Games { get; }
    public IGameSessionRepository GameSessions { get; }
    public ISessionLogRepository SessionLogs { get; }

    public EfUnitOfWork(AppDbContext db)
    {
        _db = db;
        Users = new UserRepository(db);
        Games = new GameRepository(db);
        GameSessions = new GameSessionRepository(db);
        SessionLogs = new SessionLogRepository(db);
    }

    public async Task BeginTransactionAsync(CancellationToken ct = default)
        => _tx = await _db.Database.BeginTransactionAsync(ct);

    public async Task CommitAsync(CancellationToken ct = default)
    {
        if (_tx is not null) await _tx.CommitAsync(ct);
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_tx is not null) await _tx.RollbackAsync(ct);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);

    public async ValueTask DisposeAsync()
    {
        if (_tx is not null) await _tx.DisposeAsync();
        await _db.DisposeAsync();
    }
}

