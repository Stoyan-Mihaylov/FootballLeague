using FootballLeague.Application.Models.Teams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballLeague.Application.Contracts.Services
{
    public interface ITeamService
    {
        Task<List<GetAllTeamsResponse>> GetAllTeamsAsync();

        Task<List<TeamsRankingResponse>> GetTeamsRankingAsync();

        Task<TeamResponse> CreateTeamAsync(TeamRequest request);

        Task UpdateTeamAsync(int teamId, TeamRequest request);

        Task DeleteTeamAsync(int teamId);
    }
}
