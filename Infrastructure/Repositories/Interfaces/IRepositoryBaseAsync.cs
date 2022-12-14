using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Domains;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IRepositoryBase<T, K, TContext>
        where T : EntityBase<K>
        where TContext : DbContext
    {
        IQueryable<T> FindAll(bool trackChanges = false);
        IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
            params Expression<Func<T, object>>[] includeProperties);

        Task<T?> FindAsync(K id);
        Task<T?> FindAsync(K id, params Expression<Func<T, object>>[] includeProperties);
        void Add(T entity);
        Task<K> AddAsync(T entity);
        void Update(T entity);
        Task UpdateAsync(T entity);
        void Remove(T entity);
        Task RemoveAsync(T entity);
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task EndTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
