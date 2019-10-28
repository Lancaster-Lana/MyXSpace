using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyXSpace.Core.Interfaces;
using MyXSpace.EF;

namespace MyXSpace.Core.Base
{
    /// <summary>
    /// Repository to manage tenants catalog DB
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class CRepository<TEntity> : CRepository<TEntity, int> where TEntity : class
    {
        public CRepository(CatalogDbContext context): base(context)
        {
            //_context = context;
            //_entities = context.Set<TEntity>();
        }
    }

    /// <summary>
    /// Repository for Catalog (host of tenants) DB only
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class CRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly CatalogDbContext _context;
        protected readonly DbSet<TEntity> _entities;

        public CRepository(CatalogDbContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate);
        }

        public virtual TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.SingleOrDefault(predicate);
        }

        public virtual TEntity Get(TKey id)
        {
            return _entities.Find(id);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _entities;
        }

        public virtual void Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _entities.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _entities.UpdateRange(entities);
        }

        public virtual void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public virtual int Count()
        {
            return _entities.Count();
        }

    }
}
