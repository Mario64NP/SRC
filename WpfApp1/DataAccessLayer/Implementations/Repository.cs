using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WpfApp.DataAccessLayer.Interfaces;

namespace WpfApp.DataAccessLayer.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TEntity GetById(int id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().Local.ToObservableCollection();
        }
        
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Where(predicate);
        }

        public void Add(TEntity t)
        {
            _dbContext.Add(t);
        }

        public void Remove(TEntity t)
        {
            _dbContext.Remove(t);
        }
    }
}