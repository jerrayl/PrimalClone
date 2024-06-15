using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Primal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Primal.Repositories
{
    public class DatabaseRepository<T> : IDatabaseRepository<T> where T : BaseEntity
    {
        private readonly DatabaseContext _dbContext;
        private readonly DbSet<T> _entities;

        public DatabaseRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            _entities = dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _entities.Add(entity);
            _dbContext.SaveChanges();
        }

        public void AddBatch(List<T> entities)
        {
            if (entities.Any(e => e is null))
                throw new ArgumentNullException(nameof(entities));
            _entities.AddRange(entities);
            _dbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _entities.Update(entity);
            _dbContext.SaveChanges();
        }

        public void UpdateBatch(List<T> entities)
        {
            if (entities.Any(e => e is null))
                throw new ArgumentNullException(nameof(entities));
            _entities.UpdateRange(entities);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _entities.Remove(entity);
            _dbContext.SaveChanges();
        }

        public void DeleteBatch(List<T> entities)
        {
            _entities.RemoveRange(entities);
            _dbContext.SaveChanges();
        }

        public IEnumerable<T> Read()
        {
            return _entities.AsEnumerable();
        }

        public T ReadOne(Func<T, bool> predicate,
             params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> dbQuery = _dbContext.Set<T>();

            foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<T, object>(navigationProperty);

            return dbQuery.Where(predicate).SingleOrDefault();
        }

        public IEnumerable<T> Read(Func<T, bool> predicate,
             params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> dbQuery = _dbContext.Set<T>();

            foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<T, object>(navigationProperty);

            return dbQuery.Where(predicate);
        }

        public int Count()
        {
            return _entities.AsEnumerable().Count();
        }
    }
}