using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> Filter, string? includeProperities = null, bool tracked = false);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperities = null);
        Task<IEnumerable<T>> GetAllPaginatedFilterAsync(Expression<Func<T, bool>>? filter , int page = 0 , int pageSize = 0);
        Task AddAsync(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

       

    }
}
