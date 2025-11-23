using FootballLeague.Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Application.Contracts.Repositories
{
    public interface IMatchRepository : IRepository<Match, int>
    {
        Task<bool> ExistsAsync(Expression<Func<Match, bool>> predicate, CancellationToken cancellationToken);
    }
}
