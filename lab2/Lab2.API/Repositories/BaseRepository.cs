using Lab2.Abstractions;
using Lab2.Data;
using Microsoft.EntityFrameworkCore;
namespace Lab2.Repositories;
public abstract class BaseRepository<TEntity, TId>(AppDbContext db) : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : struct
{
    protected readonly DbSet<TEntity> Set = db.Set<TEntity>();
    public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await Set.AsNoTracking().ToListAsync();
    public async Task<TEntity?> GetByIdAsync(TId id)
        => await Set.FindAsync(id);
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await Set.AddAsync(entity);
        return entity;
    }
    public async Task<TEntity?> UpdateAsync(TId id, TEntity entity)
    {
        var existing = await Set.FindAsync(id);
        if (existing is null) return null;
        db.Entry(existing).CurrentValues.SetValues(entity);
        return existing;
    }
    public async Task<bool> DeleteAsync(TId id)
    {
        var entity = await Set.FindAsync(id);
        if (entity is null) return false;
        Set.Remove(entity);
        return true;
    }
    public async Task<bool> ExistsByIdAsync(TId id)
        => await Set.AnyAsync(e => e.Id.Equals(id));
}