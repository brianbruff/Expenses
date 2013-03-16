using System;
using System.Linq;
using System.Linq.Expressions;

namespace Expenses.Data.Contracts
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);

        
        IRepository<T> Include<TProperty>(Expression<Func<T, TProperty>> path);
    }
}