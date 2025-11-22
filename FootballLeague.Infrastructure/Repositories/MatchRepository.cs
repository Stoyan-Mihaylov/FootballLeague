using FootballLeague.Application.Contracts.Repositories;
using FootballLeague.Application.Exceptions;
using FootballLeague.Domain.Entities;
using FootballLeague.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FootballLeague.Infrastructure.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly FootballLeagueDbContext _context;

        public MatchRepository(FootballLeagueDbContext context) => _context = context;

        public async Task<List<Match>> GetAllAsync() =>
            await _context.Matches
            .AsNoTracking()
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .ToListAsync();

        public async Task<Match> GetByConditionAsync(
            Expression<Func<Match, bool>> predicate,
            bool isTracked = true)
        {
            var match = await _context.Matches
                .WithTracking(isTracked)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Where(predicate)
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException("Match not found!");

            return match;
        }

        public async Task AddAsync(Match entity) => await _context.Matches.AddAsync(entity);

        public void Update(Match entity) => _context.Matches.Update(entity);

        public void Delete(Match entity)
        {
            _context.Matches.Remove(entity);
        }
    }
}
