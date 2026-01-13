using IdentityCore.EFs;
using IdentityCore.EFs.Entities;
using IdentityCore.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace IdentityCore.Repository.Base
{
    public abstract class Repository<T> : IRepository<T> where T : class, IBaseEntity
    {
        protected IUnitOfWork _unitOfWork;
        protected DbSet<T> DbSet = null;
        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            DbSet = _unitOfWork.identityContext.Set<T>();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = DbSet.AsNoTracking();

            if (includeProperties.Length > 0)
            {
                includeProperties = includeProperties.Distinct().ToArray();
            }

            if (includeProperties?.Any() == true)
            {
                foreach (var includeProperty in includeProperties)
                    query = query.Include(includeProperty);
            }

            return predicate == null ? query : query.Where(predicate);
        }

        public T GetSingle(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            return Get(predicate, includeProperties).FirstOrDefault();
        }

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
        {
            var query = DbSet.AsNoTracking();
            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);
            return query;
        }

        public T Add(T entity)
        {
            entity.DateCreated = DateTime.UtcNow;
            entity.DateModified = DateTime.UtcNow;
            entity.GUID = Guid.NewGuid();
            entity.IsDeleted = false;

            entity = _unitOfWork.identityContext.Add(entity).Entity;
            return entity;
        }

        public void Update(T entity, params Expression<Func<T, object>>[] changedProperties)
        {
            TryAttach(entity);

            if(changedProperties.Length > 0)
            {
                changedProperties = changedProperties.Distinct().ToArray();
            }           

            if (changedProperties?.Any() == true)
            {
                foreach (var property in changedProperties)
                {
                    _unitOfWork.identityContext.Entry(entity).Property(property).IsModified = true;
                }

                entity.DateModified = DateTime.UtcNow;
            }
            else
            {
                _unitOfWork.identityContext.Entry(entity).State = EntityState.Modified;
                entity.DateModified = DateTime.UtcNow;
            }
        }

        public void Delete(T entity, bool isPhysical = false)
        {
            try
            {
                TryAttach(entity);

                if (!isPhysical)
                {
                    entity.IsDeleted = true;
                    entity.DateModified = DateTime.Now;

                    _unitOfWork.identityContext.Entry(entity).Property(nameof(entity.IsDeleted)).IsModified = true;
                }
                else
                {
                    _unitOfWork.identityContext.Remove(entity);
                }
            }
            catch (Exception)
            {
                RefreshEntity(entity);
                throw;
            }
        }

        public void DeleteWhere(Expression<Func<T, bool>> predicate, bool isPhysical = false)
        {
            var entities = Get(predicate).AsEnumerable();

            foreach (var entity in entities)
                Delete(entity, isPhysical);
        }

        private bool TryAttach(T entity)
        {
            try
            {
                if (_unitOfWork.identityContext.Entry(entity).State == EntityState.Detached)
                    _unitOfWork.identityContext.Attach(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void RefreshEntity(T entity)
        {
            _unitOfWork.identityContext.Entry(entity).Reload();
        }
    }
}
