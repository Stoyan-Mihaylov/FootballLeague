using FootballLeague.Application.Contracts.Repositories;
using FootballLeague.Application.Exceptions;
using FootballLeague.Application.Models.Teams;
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
    public class TeamRepository : ITeamRepository
    {
        private readonly FootballLeagueDbContext _context;

        public TeamRepository(FootballLeagueDbContext context) => _context = context;

        public async Task<List<Team>> GetAllAsync(CancellationToken cancellationToken) => await _context
            .Teams
            .AsNoTracking()
            .Include(t => t.HomeMatches)
            .Include(t => t.AwayMatches)
            .OrderByDescending(t => t.Points)
            .ToListAsync(cancellationToken);

        public async Task<Team> GetByConditionAsync(
            Expression<Func<Team, bool>> predicate,
            CancellationToken cancellationToken,
            bool isTracked = true)
        {
            var team = await _context.Teams
                .WithTracking(isTracked)
                .Where(predicate)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Team not found!");

            return team;
        }

        public async Task AddAsync(Team entity, CancellationToken cancellationToken) => await _context.Teams.AddAsync(entity, cancellationToken);

        public void Delete(Team entity) => _context.Teams.Remove(entity);

        public async Task<List<TeamsRankingResponse>> GetTeamsRankingAsync(CancellationToken cancellationToken)
        {
            return await _context.Teams
                .AsNoTracking()
                .Include(t => t.HomeMatches)
                .Include(t => t.AwayMatches)
                .Select(team => new TeamsRankingResponse
                {
                    TeamId = team.Id,
                    TeamName = team.Name,

                    Played = team.HomeMatches.Count + team.AwayMatches.Count,
                    HomePlayed = team.HomeMatches.Count,
                    AwayPlayed = team.AwayMatches.Count,

                    Won =
                        team.HomeMatches.Count(m => m.HomeScore > m.AwayScore) +
                        team.AwayMatches.Count(m => m.AwayScore > m.HomeScore),

                    Draw =
                        team.HomeMatches.Count(m => m.HomeScore == m.AwayScore) +
                        team.AwayMatches.Count(m => m.AwayScore == m.HomeScore),

                    Lost =
                        team.HomeMatches.Count(m => m.HomeScore < m.AwayScore) +
                        team.AwayMatches.Count(m => m.AwayScore < m.HomeScore),

                    GoalsScored =
                        (team.HomeMatches.Sum(m => (int?)m.HomeScore) ?? 0) +
                        (team.AwayMatches.Sum(m => (int?)m.AwayScore) ?? 0),

                    GoalsConceded =
                        (team.HomeMatches.Sum(m => (int?)m.AwayScore) ?? 0) +
                        (team.AwayMatches.Sum(m => (int?)m.HomeScore) ?? 0),

                    Points = team.Points
                })
                .OrderByDescending(r => r.Points)
                .ThenByDescending(r => r.Won)
                .ToListAsync(cancellationToken);
        }
    }
}
