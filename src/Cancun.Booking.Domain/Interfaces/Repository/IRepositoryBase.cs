using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Domain.Interfaces.Repository
{
    public interface IRepositoryBase<T>
        where T : class
    {
        bool Any(Expression<Func<T, bool>> predicate);

        void Delete(object id);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> predicate);

        T GetBy(Expression<Func<T, bool>> predicate);

        T GetById(object id);

        void Insert(T obj);

        void Save();
    }
}
