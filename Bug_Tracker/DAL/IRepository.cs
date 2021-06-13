using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.DAL
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetEntity(int id);
        T GetEntity(Func<T, bool> condition);
        IEnumerable<T> GetCollection(Func<T, bool> condition);
    }
}