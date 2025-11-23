using FootballLeague.Application.Models.Teams;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Application.Contracts.Services
{
    public interface ITeamService
    {
        Task<List<GetAllTeamsResponse>> GetAllTeamsAsync(CancellationToken cancellationToken);

        Task<List<TeamsRankingResponse>> GetTeamsRankingAsync(CancellationToken cancellationToken);

        Task<TeamResponse> CreateTeamAsync(TeamRequest request, CancellationToken cancellationToken);

        Task UpdateTeamAsync(int teamId, TeamRequest request, CancellationToken cancellationToken);

        Task DeleteTeamAsync(int teamId, CancellationToken cancellationToken);
    }
}
