using System.Linq.Expressions;
using ChatApp.Application.Common.Persistence;
using ChatApp.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Persistence.Repositories;

///<inheritdoc cref="IRepository{TEntity}"/>
public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private protected readonly ChatContext _context;
    private protected readonly DbSet<TEntity> _dbSet;

    protected BaseRepository(ChatContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null)
    {
        if (filter is null)
        {
            return await _dbSet.ToListAsync();
        }

        return await _dbSet.Where(filter).ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(int entityId)
    {
        return await _dbSet.FindAsync(entityId);
    }

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public async Task DeleteByIdAsync(int entityId)
    {
        var entityToDelete = await _dbSet.FindAsync(entityId);

        if (entityToDelete is not null)
        {
            _dbSet.Remove(entityToDelete);
        }
    }
}