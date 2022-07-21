using System.Linq.Expressions;
using ChatApp.Domain.Common;

namespace ChatApp.Application.Common.Persistence;

/// <summary>
/// Represents a base repository.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IRepository<TEntity> where TEntity: BaseEntity
{
    /// <summary>
    /// Gets data by filter or all data if filter is not specified.
    /// </summary>
    /// <param name="filter">Data filter.</param>
    Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null);

    /// <summary>
    /// Gets an entity with the specified id.
    /// </summary>
    /// <param name="entityId">Entity id.</param>
    /// <returns>Entity with specified id.</returns>
    Task<TEntity> GetByIdAsync(int entityId);

    /// <summary>
    /// Adds new entity.
    /// </summary>
    /// <param name="entity">Entity to add.</param>
    void Add(TEntity entity);

    /// <summary>
    /// Updates an entity.
    /// </summary>
    /// <param name="entity">Entity to update.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Deletes an entity with the specified id.
    /// </summary>
    /// <param name="entityId">Entity id.</param>
    Task DeleteByIdAsync(int entityId);
}