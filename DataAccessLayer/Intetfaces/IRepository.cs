using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccessLayer.Intetfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get entity from appropriate table by ID
        /// </summary>
        /// <param name="id">ID of searching entity</param>
        TEntity Get(int id);
        
        /// <summary>
        /// Get all entities from appropriate table
        /// </summary>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Get entities, which satisfies predicate
        /// </summary>
        /// <param name="predicate">Logic expression, which should satisfy searching enteties</param>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        
        /// <summary>
        /// Add entity to appropriate table
        /// </summary>
        /// <param name="entity">New entity, which adding to table</param>
        void Add(TEntity entity);
        
        /// <summary>
        /// Add range of entities to appropriate table
        /// </summary>
        /// <param name="entities">Collection of entities to appropriate table</param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Mark entity in appropriate table as modified
        /// </summary>
        /// <param name="entity">Entity, which was modified</param>
        void Update(TEntity entity);

        /// <summary>
        /// Remove entity from appropriate table
        /// </summary>
        /// <param name="entity">Entity for remove</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Remove range of entities
        /// </summary>
        /// <param name="entities">Collection of entities for remove</param>
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
