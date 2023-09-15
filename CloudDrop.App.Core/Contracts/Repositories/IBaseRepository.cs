using CloudDrop.App.Core.Entities;
using CloudDrop.App.Core.Entities.Base;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace CloudDrop.App.Core.Contracts.Repositories;
public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    CloudDropDbContext DbContext { get; }
    DbSet<TEntity> Entity { get; }

    Task DeleteAsync(IEnumerable<TEntity> entities);
    Task DeleteAsync(TEntity entity);
    Task DeleteByGuidAsync(Guid guid);
    Task DeleteByIdAsync(int id);
    IEnumerable<TEntity> Get(Func<TEntity, bool> where);
    IEnumerable<TEntity> GetAll();
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<TEntity>?> GetAsync(Expression<Func<TEntity, bool>> where, bool tracking = true, CancellationToken ct = default);
    Task<TEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
    Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<TEntity?> GetFirstOrDefaultAsync(bool tracking = true, CancellationToken ct = default);
    Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, bool tracking = true, CancellationToken ct = default);
    Task<TEntity?> GetLastOrDefaultAsync(bool tracking = true, CancellationToken ct = default);
    Task<TEntity?> GetLastOrDefaultAsync(Expression<Func<TEntity, bool>> where, bool tracking = true, CancellationToken ct = default);
    Task InsertAsync(IEnumerable<TEntity> entities);
    Task InsertAsync(TEntity entity);
    Task UpdateAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
