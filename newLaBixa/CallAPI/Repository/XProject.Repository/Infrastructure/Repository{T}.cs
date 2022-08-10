using Invedia.Data.EF.Interfaces.DbContext;
using Invedia.Data.EF.Services.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using XProject.Contract.Repository.Infrastructure;
using XProject.Contract.Repository.Models;
using XProject.Core.Utils;

namespace XProject.Repository.Infrastructure
{
    //EntityStringRepository<T> where T : Entity, new()
    public class Repository<T> : IRepository<T> where T : Entity, new()
    {
        protected string ConnectionString;
        private readonly IDbContext _context;

        //private readonly SqlGeneratorConfig _config = new SqlGeneratorConfig { SqlProvider = SqlProvider.Oracle };

        //protected
        public Repository(IDbContext dbContext)
        {
            _context = dbContext;
            ConnectionString = SystemHelper.AppDb;
        }

        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public List<T> AddRange(params T[] entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity, bool isPhysicalDelete = false)
        {
            throw new NotImplementedException();
        }

        public void DeleteWhere(List<string> ids, bool isPhysicalDelete = false)
        {
            throw new NotImplementedException();
        }

        public void DeleteWhere(Expression<Func<T, bool>> predicate, bool isPhysicalDelete = false)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate = null, bool isIncludeDeleted = false, params Expression<Func<T, object>>[] includeProperties)
        {
            return _context.Set<T>();
        }

        public T GetSingle(Expression<Func<T, bool>> predicate = null, bool isIncludeDeleted = false, params Expression<Func<T, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public void RefreshEntity(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity, params Expression<Func<T, object>>[] changedProperties)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity, params string[] changedProperties)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateWhere(Expression<Func<T, bool>> predicate, T entityNewData, params Expression<Func<T, object>>[] changedProperties)
        {
            throw new NotImplementedException();
        }

        public void UpdateWhere(Expression<Func<T, bool>> predicate, T entityNewData, params string[] changedProperties)
        {
            throw new NotImplementedException();
        }
    }
}