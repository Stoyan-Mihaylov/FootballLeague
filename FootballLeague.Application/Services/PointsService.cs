using FootballLeague.Application.Contracts.Services;
using FootballLeague.Domain.Entities;
using FootballLeague.Domain.Enums;

namespace FootballLeague.Application.Services
{
    public class PointsService : IPointsService
    {
        private const int PointsForWin = 3;
        private const int PointsForDraw = 1;
        private const int PointsForLoss = 0;

        public void UpdateTeamsPointsBasedOnMatch(Match match, Team homeTeam, Team awayTeam, PointsAction action)
        {
            var (homePoints, awayPoints) = CalculatePoints(match);

            if (action == PointsAction.Revert)
                (homePoints, awayPoints) = RollbackPoints(homePoints, awayPoints);

            ApplyPoints(homeTeam, awayTeam, homePoints, awayPoints);
        }

        private (int homePoints, int awayPoints) CalculatePoints(Match match)
        {
            int home = match.HomeScore;
            int away = match.AwayScore;

            return home switch
            {
                _ when home == away => (PointsForDraw, PointsForDraw),
                _ when home > away => (PointsForWin, PointsForLoss),
                _ => (PointsForLoss, PointsForWin)
            };
        }

        private (int home, int away) RollbackPoints(int home, int away)
            => (-home, -away);

        private void ApplyPoints(Team homeTeam, Team awayTeam, int homePoints, int awayPoints)
        {
            homeTeam.Points += homePoints;
            awayTeam.Points += awayPoints;
        }
    }
}
