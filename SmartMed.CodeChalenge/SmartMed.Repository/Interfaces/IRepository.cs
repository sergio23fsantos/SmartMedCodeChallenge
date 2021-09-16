using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SmartMed.Repository.Interfaces
{
    public interface IRepository<T>
    {
        T Add(T item);

        T Update(T item);

        void Delete(object id);

        T Get(Expression<Func<T, bool>> query);

        IEnumerable<T> Get();

        IEnumerable<T> Query(Expression<Func<T, bool>> query);
    }
}
