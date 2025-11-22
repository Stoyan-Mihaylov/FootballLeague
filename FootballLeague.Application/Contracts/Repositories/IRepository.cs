using FootballLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FootballLeague.Application.Contracts.Repositories
{
    public interface IRepository<TEntity, TKey> 
        where TEntity : BaseEntity<TKey>
    {
        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate, bool isTracked = true);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
