using CloudDrop.App.Core.Contracts.Repositories;
using CloudDrop.App.Core.Entities;
using CloudDrop.App.Core.Entities.Base;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace CloudDrop.App.Core.Repositories.Base;


public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    public CloudDropDbContext DbContext => _dbContext;
    public DbSet<TEntity> Entity => _entity;

    private readonly CloudDropDbContext _dbContext;
    private readonly DbSet<TEntity> _entity;

    public BaseRepository(CloudDropDbContext dbContext)
    {
        _dbContext = dbContext;
        _entity = dbContext.Set<TEntity>();
    }

    public IEnumerable<TEntity> GetAll()
    {
        return _entity.AsEnumerable();
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _entity.ToListAsync(ct);
    }
    public IEnumerable<TEntity> Get(Func<TEntity, bool> where)
    {
        return _entity.Where(where).ToList();
    }

    public async Task<IEnumerable<TEntity>?> GetAsync(Expression<Func<TEntity, bool>> where, bool tracking = true,
        CancellationToken ct = default)
    {
        return await _entity.Where(where)
            .AsTracking(tracking ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
            .ToListAsync(ct);
    }

    public async Task<TEntity?> GetFirstOrDefaultAsync(bool tracking = true,
        CancellationToken ct = default)
    {
        return await _entity
            .AsTracking(tracking ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, bool tracking = true,
        CancellationToken ct = default)
    {
        return await _entity
            .AsTracking(tracking ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
            .FirstOrDefaultAsync(where, ct);
    }

    public async Task<TEntity?> GetLastOrDefaultAsync(bool tracking = true,
        CancellationToken ct = default)
    {
        return await _entity
            .AsTracking(tracking ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
            .LastOrDefaultAsync(ct);
    }

    public async Task<TEntity?> GetLastOrDefaultAsync(Expression<Func<TEntity, bool>> where, bool tracking = true,
        CancellationToken ct = default)
    {
        return await _entity
            .AsTracking(tracking ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
            .LastOrDefaultAsync(where, ct);
    }

    public async Task InsertAsync(TEntity entity)
    {
        await _entity.AddAsync(entity);
    }

    public async Task InsertAsync(IEnumerable<TEntity> entities)
    {
        await _entity.AddRangeAsync(entities);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await Task.Run(() => _entity.Update(entity));
    }
    public async Task UpdateAsync(IEnumerable<TEntity> entities)
    {
        await Task.Run(() => _entity.UpdateRange(entities));
    }

    public async Task DeleteAsync(TEntity entity)
    {
        await Task.Run(() => _entity.Remove(entity));
    }
    public async Task DeleteAsync(IEnumerable<TEntity> entities)
    {
        await Task.Run(() => _entity.RemoveRange(entities));
    }


    public async Task DeleteByIdAsync(int id)
    {
        var entity = await GetFirstOrDefaultAsync(e => e.Id == id, tracking: true);
        if (entity is null)
        {
            return;
        }
        await DeleteAsync(entity);
    }
    public async Task DeleteByGuidAsync(Guid guid)
    {
        var entity = await GetFirstOrDefaultAsync(e => e.Guid == guid, tracking: true);
        if (entity is null)
        {
            return;
        }
        await DeleteAsync(entity);
    }

    public async Task<TEntity?> GetByIdAsync(int id,
        CancellationToken ct = default)
    {
        return await _entity.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<TEntity?> GetByGuidAsync(Guid guid,
        CancellationToken ct = default)
    {
        return await _entity.FirstOrDefaultAsync(x => x.Guid == guid, ct);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _dbContext.SaveChangesAsync(ct);
    }
}
