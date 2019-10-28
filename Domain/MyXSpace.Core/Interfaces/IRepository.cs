
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyXSpace.Core.Interfaces
{
    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : class
    {
    }

    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        TEntity Get(TKey id);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        void SaveChanges();

        int Count();
    }
}
