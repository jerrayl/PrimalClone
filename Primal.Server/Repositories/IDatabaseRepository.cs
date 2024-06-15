using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Primal.Entities;

namespace Primal.Repositories
{
    public interface IDatabaseRepository<T> where T : BaseEntity
    {
        void Add(T entity);

        void AddBatch(List<T> entities);

        void Update(T entity);

        void UpdateBatch(List<T> entities);

        void Delete(T entity);

        void DeleteBatch(List<T> entities);

        IEnumerable<T> Read();

        T ReadOne(Func<T, bool> predicate, params Expression<Func<T, object>>[] navigationProperties);

        IEnumerable<T> Read(Func<T, bool> predicate, params Expression<Func<T, object>>[] navigationProperties);

        int Count();
    }
}