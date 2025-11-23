using FootballLeague.Application.Contracts.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FootballLeagueDbContext _context;

        public UnitOfWork(FootballLeagueDbContext context)
        {
            _context = context;
            Teams = new TeamRepository(_context);
            Matches = new MatchRepository(_context);
        }

        public ITeamRepository Teams { get; }
        public IMatchRepository Matches { get; }

        public async Task SaveChangesAsync(CancellationToken cancellationToken) => await _context.SaveChangesAsync(cancellationToken);
    }
}
