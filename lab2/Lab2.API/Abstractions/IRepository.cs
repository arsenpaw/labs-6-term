namespace Lab2.Abstractions;
public interface IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : struct
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(TId id);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity?> UpdateAsync(TId id, TEntity entity);
    Task<bool> DeleteAsync(TId id);
    Task<bool> ExistsByIdAsync(TId id);
}