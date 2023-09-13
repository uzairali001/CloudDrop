using CloudDrop.Api.Core.Contracts.Repositories;
using CloudDrop.Api.Core.Entities;
using CloudDrop.Api.Core.Entities.Base;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace CloudDrop.Api.Core.Repositories.Base;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly CloudDropDbContext _dbContext;
    protected readonly DbSet<TEntity> _entity;
    protected readonly IQueryable<TEntity> _queryBuilder;

    public BaseRepository(CloudDropDbContext dbContext)
    {
        _dbContext = dbContext;
        _entity = dbContext.Set<TEntity>();
        _queryBuilder = _entity.AsQueryable<TEntity>();
    }

    #region Add
    public async Task AddAsync(TEntity entity, CancellationToken cancellation = default)
    {
        await _entity.AddAsync(entity, cancellation);
    }

    public async Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default)
    {
        await _entity.AddRangeAsync(entities, cancellation);
    }

    public async Task<bool> AddAndSaveAsync(TEntity entity, CancellationToken cancellation = default)
    {
        await _entity.AddAsync(entity, cancellation);
        return await SaveChangesAsync(cancellation) > 0;
    }

    public async Task<bool> AddAndSaveAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default)
    {
        await _entity.AddRangeAsync(entities, cancellation);
        return await SaveChangesAsync(cancellation) > 0;
    }
    #endregion

    #region Delete
    public void Delete(TEntity entity)
    {
        _entity.Remove(entity);
    }

    public void Delete(IEnumerable<TEntity> entities)
    {
        _entity.RemoveRange(entities);
    }

    public async Task<bool> DeleteAndSaveAsync(TEntity entity, CancellationToken cancellation = default)
    {
        _entity.Remove(entity);
        return await SaveChangesAsync(cancellation) > 0;
    }

    public async Task<bool> DeleteAndSaveAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default)
    {
        _entity.RemoveRange(entities);
        return await SaveChangesAsync(cancellation) > 0;
    }
    #endregion

    #region Get
    private IQueryable<TEntity> InternalGet(bool asTracking)
    {
        return _entity
            .AsTracking(asTracking ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
            .Where(e => e.IsDeleted == false);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(bool asTracking = false, CancellationToken cancellation = default)
    {
        return await InternalGet(asTracking)
            .ToListAsync(cancellation);
    }

    public async Task<IEnumerable<TEntity>?> GetAsync(Expression<Func<TEntity, bool>> predicate, bool asTracking = false, CancellationToken cancellation = default)
    {
        return await InternalGet(asTracking)
            .Where(predicate)
            .ToListAsync(cancellationToken: cancellation);
    }

    public async Task<TEntity?> GetByIdAsync(uint id, bool asTracking = false, CancellationToken cancellation = default)
    {
        return await InternalGet(asTracking)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken: cancellation);
    }

    public async Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, bool asTracking = false, CancellationToken cancellation = default)
    {
        return await InternalGet(asTracking)
            .FirstOrDefaultAsync(predicate, cancellationToken: cancellation);
    }

    public async Task<TEntity?> GetLastAsync(Expression<Func<TEntity, bool>> predicate, bool asTracking = false, CancellationToken cancellation = default)
    {
        return await InternalGet(asTracking)
            .OrderByDescending(x => x)
            .FirstOrDefaultAsync(predicate, cancellationToken: cancellation);
    }

    public async Task<TEntity?> GetLatestByAsync(DateTime dateTime, bool asTracking = false, CancellationToken cancellation = default)
    {
        return await InternalGet(asTracking)
            .FirstOrDefaultAsync(e => e.UpdatedAt >= dateTime, cancellationToken: cancellation);
    }
    #endregion

    #region Update
    public async Task UpdateAsync(TEntity entity, CancellationToken cancellation = default)
    {
        await Task.Run(() => _entity.Update(entity), cancellation);
    }

    public async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default)
    {
        await Task.Run(() => _entity.UpdateRange(entities), cancellation);
    }
    public async Task<bool> UpdateAndSaveAsync(TEntity entity, CancellationToken cancellation = default)
    {
        _entity.Update(entity);
        return await SaveChangesAsync(cancellation) > 0;
    }

    public async Task<bool> UpdateAndSaveAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default)
    {
        _entity.UpdateRange(entities);
        return await SaveChangesAsync(cancellation) > 0;
    }
    #endregion

    public async Task<int> SaveChangesAsync(CancellationToken cancellation = default)
    {
        return await _dbContext.SaveChangesAsync(cancellation);
    }
}
