using FootballLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Application.Contracts.Repositories
{
    public interface IRepository<TEntity, TKey> 
        where TEntity : BaseEntity<TKey>
    {
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);

        Task<TEntity> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, bool isTracked = true);

        Task AddAsync(TEntity entity, CancellationToken cancellationToken);

        void Delete(TEntity entity);
    }
}
