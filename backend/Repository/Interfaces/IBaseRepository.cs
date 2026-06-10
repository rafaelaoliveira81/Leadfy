
public interface IBaseRepository<TEntity> where TEntity : class, IEntity
{
    Task<TEntity> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(TEntity entity);
    Task<IEnumerable<Guid>> CreateManyAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity);
    Task UpdateManyAsync(IEnumerable<TEntity> entities);
    Task DeleteAsync(TEntity entity);
    Task<IEnumerable<TEntity>> GetAllAsync();
}