using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Homework2.DAL.Abstract
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        TEntity GetById(int id);
        bool Exists(int id);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Getll(params Expression<Func<TEntity, object>>[] includes);
        TEntity AddOrUpdate(TEntity entity);
        void Delete(TEntity entity);
        void DeleteById(int id);
    }
    
}