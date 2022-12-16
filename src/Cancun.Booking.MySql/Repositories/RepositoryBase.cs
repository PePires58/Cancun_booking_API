using Cancun.Booking.Domain.Interfaces.Repository;
using Cancun.Booking.MySql.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Cancun.Booking.MySql.Repositories
{
    [ExcludeFromCodeCoverage]
    public class RepositoryBase<T> : IRepositoryBase<T>, IDisposable
        where T: class
    {
        #region Readonly
        public readonly CancunDbContext CancunDbContext;
        protected readonly DbSet<T> Items;
        #endregion

        #region Constructor

        public RepositoryBase(CancunDbContext cancunDbContext)
        {
            CancunDbContext = cancunDbContext;
            Items = CancunDbContext.Set<T>();
        }
        #endregion

        #region Methods
        public bool Any(Expression<Func<T, bool>> predicate) => Items.Any(predicate);
        
        public void Delete(object id)
        {
            if (id != null)
            {
                T item = GetById(id);
                
                if (item != null)
                    Items.Remove(item);
            }
        }

        public virtual IEnumerable<T> GetAll() => Items.ToList();

        public virtual IEnumerable<T> GetAllBy(Expression<Func<T, bool>> predicate) =>
            Items.Where(predicate).ToList();

        public virtual T GetBy(Expression<Func<T, bool>> predicate) =>
            Items.AsNoTracking().FirstOrDefault(predicate);

        public virtual T GetById(object id) => Items.Find(id);

        public void Insert(T obj) => Items.Add(obj);

        public void Update(T obj) => Items.Update(obj);

        public void Save() => CancunDbContext.SaveChanges();

        public void Dispose() => CancunDbContext.Dispose();
        #endregion
    }
}
