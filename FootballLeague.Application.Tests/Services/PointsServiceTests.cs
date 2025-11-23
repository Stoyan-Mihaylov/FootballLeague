using FootballLeague.Application.Services;
using FootballLeague.Domain.Entities;
using FootballLeague.Domain.Enums;
using NUnit.Framework;

namespace FootballLeague.Application.Tests.Services
{
    [TestFixture]
    public class PointsServiceTests
    {
        private PointsService _pointsService;

        [SetUp]
        public void Setup()
        {
            _pointsService = new PointsService();
        }

        [TestCase(2, 2, 1, 1)]
        [TestCase(3, 1, 3, 0)]
        [TestCase(0, 2, 0, 3)]
        public void UpdateTeamsPointsBasedOnMatch_ShouldApplyPointsCorrectly(int homeScore, int awayScore, int expectedHomePoints, int expectedAwayPoints)
        {
            // Arrange
            var match = new Match { HomeScore = homeScore, AwayScore = awayScore };
            var homeTeam = new Team { Points = 0 };
            var awayTeam = new Team { Points = 0 };

            // Act
            _pointsService.UpdateTeamsPointsBasedOnMatch(match, homeTeam, awayTeam, PointsAction.Apply);

            // Assert
            Assert.AreEqual(expectedHomePoints, homeTeam.Points);
            Assert.AreEqual(expectedAwayPoints, awayTeam.Points);
        }

        [TestCase(2, 2, -1, -1)]
        [TestCase(3, 1, -3, 0)]
        [TestCase(0, 2, 0, -3)]
        public void UpdateTeamsPointsBasedOnMatch_WithRevert_ShouldRollbackPointsCorrectly(int homeScore, int awayScore, int expectedHomePoints, int expectedAwayPoints)
        {
            // Arrange
            var match = new Match { HomeScore = homeScore, AwayScore = awayScore };
            var homeTeam = new Team { Points = 5 };
            var awayTeam = new Team { Points = 3 };

            // Act
            _pointsService.UpdateTeamsPointsBasedOnMatch(match, homeTeam, awayTeam, PointsAction.Revert);

            // Assert
            Assert.AreEqual(5 + expectedHomePoints, homeTeam.Points);
            Assert.AreEqual(3 + expectedAwayPoints, awayTeam.Points);
        }
    }
}
