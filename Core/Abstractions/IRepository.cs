﻿using System;
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
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperities = null);
        Task<IEnumerable<T>> GetAllPaginatedFilterAsync(Expression<Func<T, bool>>? filter = null , int page = 1 , int pageSize = 5, string? includeProperities = null);
        Task AddAsync(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

       

    }
}
