using FootballLeague.Application.Contracts.Repositories;
using FootballLeague.Application.Exceptions;
using FootballLeague.Domain.Entities;
using FootballLeague.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Infrastructure.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly FootballLeagueDbContext _context;

        public MatchRepository(FootballLeagueDbContext context) => _context = context;

        public async Task<List<Match>> GetAllAsync(CancellationToken cancellationToken) =>
            await _context.Matches
            .AsNoTracking()
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .ToListAsync(cancellationToken);

        public async Task<Match> GetByConditionAsync(
            Expression<Func<Match, bool>> predicate,
            CancellationToken cancellationToken,
            bool isTracked = true)
        {
            var match = await _context.Matches
                .WithTracking(isTracked)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Where(predicate)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Match not found!");

            return match;
        }

        public async Task AddAsync(Match entity, CancellationToken cancellationToken) => await _context.Matches.AddAsync(entity, cancellationToken);

        public void Delete(Match entity) => _context.Matches.Remove(entity);

        public async Task<bool> ExistsAsync(Expression<Func<Match, bool>> predicate, CancellationToken cancellationToken)
            => await _context.Matches.AnyAsync(predicate, cancellationToken);
    }
}
