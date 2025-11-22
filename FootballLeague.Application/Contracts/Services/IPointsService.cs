using FootballLeague.Domain.Entities;
using FootballLeague.Domain.Enums;

namespace FootballLeague.Application.Contracts.Services
{
    public interface IPointsService
    {
        void UpdateTeamsPointsBasedOnMatch(Match match, Team homeTeam, Team awayTeam, PointsAction action);
    }
}
