using FootballLeague.Application.Models.Teams;
using FootballLeague.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballLeague.Application.Contracts.Repositories
{
    public interface ITeamRepository : IRepository<Team, int>
    {
        Task<List<TeamsRankingResponse>> GetTeamsRankingAsync();
    }
}
