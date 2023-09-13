using CloudDrop.Api.Core.Entities.Base;
using System.Linq.Expressions;

namespace CloudDrop.Api.Core.Contracts.Repositories;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task<bool> AddAndSaveAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default);
    Task<bool> AddAndSaveAsync(TEntity entity, CancellationToken cancellation = default);
    Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default);
    Task AddAsync(TEntity entity, CancellationToken cancellation = default);
    void Delete(IEnumerable<TEntity> entities);
    void Delete(TEntity entity);
    Task<bool> DeleteAndSaveAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default);
    Task<bool> DeleteAndSaveAsync(TEntity entity, CancellationToken cancellation = default);
    Task<IEnumerable<TEntity>> GetAllAsync(bool asTracking = false, CancellationToken cancellation = default);
    Task<IEnumerable<TEntity>?> GetAsync(Expression<Func<TEntity, bool>> predicate, bool asTracking = false, CancellationToken cancellation = default);
    Task<TEntity?> GetByIdAsync(uint id, bool asTracking = false, CancellationToken cancellation = default);
    Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, bool asTracking = false, CancellationToken cancellation = default);
    Task<TEntity?> GetLastAsync(Expression<Func<TEntity, bool>> predicate, bool asTracking = false, CancellationToken cancellation = default);
    Task<TEntity?> GetLatestByAsync(DateTime dateTime, bool asTracking = false, CancellationToken cancellation = default);
    Task<int> SaveChangesAsync(CancellationToken cancellation = default);
    Task<bool> UpdateAndSaveAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default);
    Task<bool> UpdateAndSaveAsync(TEntity entity, CancellationToken cancellation = default);
    Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellation = default);
}
