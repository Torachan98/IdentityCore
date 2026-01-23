using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Repository.Base
{
    public interface IRepository<T>
    {
        IQueryable<T> Get(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties );

        T GetSingle(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties);

        T Add(T entity);

        List<T> AddRange(List<T> entities);

        void Update(T entity, params Expression<Func<T, object>>[] changedProperties);

        void Delete(T entity, bool isPhysical = false);

        Task<bool> Existed(Expression<Func<T, bool>> predicate = null);

        void DeleteWhere(Expression<Func<T, bool>> predicate, bool isPhysical = false);
    }
}
