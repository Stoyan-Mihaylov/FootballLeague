using FootballLeague.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballLeague.Application.Contracts.Services
{
    public interface ITeamService
    {
        Task<List<GetAllTeamsDto>> GetAllTeamsAsync();

        Task<List<TeamRankingDto>> GetTeamsRankingAsync();

        Task<TeamDto> CreateTeamAsync(TeamDto teamDto);

        Task UpdateTeamAsync(int teamId, TeamDto teamDto);

        Task DeleteTeamAsync(int teamId);
    }
}
