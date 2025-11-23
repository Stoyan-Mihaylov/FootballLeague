using AutoMapper;
using FootballLeague.Application.Contracts.Repositories;
using FootballLeague.Application.Contracts.Services;
using FootballLeague.Application.Models.Matches;
using FootballLeague.Application.Services;
using FootballLeague.Domain.Entities;
using FootballLeague.Domain.Enums;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Match = FootballLeague.Domain.Entities.Match;

namespace FootballLeague.Application.Tests.Services
{
    [TestFixture]
    public class MatchServiceTests
    {
        private Mock<IPointsService> _pointsServiceMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private MatchService _matchService;

        [SetUp]
        public void Setup()
        {
            _pointsServiceMock = new Mock<IPointsService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _matchService = new MatchService(
                _pointsServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Test]
        public async Task GetAllMatchesAsync_ShouldReturnMappedMatches()
        {
            // Arrange
            var matches = new List<Match>
            {
                new Match { Id = 1 },
                new Match { Id = 2 }
            };

            var mapped = new List<MatchResponse>
            {
                new MatchResponse { Id = 1 },
                new MatchResponse { Id = 2 }
            };

            _unitOfWorkMock
                .Setup(u => u.Matches.GetAllAsync())
                .ReturnsAsync(matches);

            _mapperMock
                .Setup(m => m.Map<List<MatchResponse>>(matches))
                .Returns(mapped);

            // Act
            var result = await _matchService.GetAllMatchesAsync();

            // Assert
            Assert.AreEqual(mapped, result);

            _unitOfWorkMock.Verify(u => u.Matches.GetAllAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<MatchResponse>>(matches), Times.Once);
        }

        [Test]
        public async Task CreateMatchAsync_ShouldCallPointsService_AndSaveMatch()
        {
            // Arrange
            var request = new MatchRequest
            {
                HomeTeamId = 1,
                AwayTeamId = 2
            };

            var homeTeam = new Team { Id = 1 };
            var awayTeam = new Team { Id = 2 };
            var match = new Match { Id = 5, HomeTeamId = 1, AwayTeamId = 2 };

            _unitOfWorkMock.SetupSequence(u => u.Teams.GetByConditionAsync(It.IsAny<Expression<Func<Team, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(homeTeam)
                .ReturnsAsync(awayTeam);

            _mapperMock.Setup(m => m.Map<Match>(request)).Returns(match);

            _unitOfWorkMock.Setup(u => u.Matches.AddAsync(match)).Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<MatchResponse>(match)).Returns(new MatchResponse { Id = match.Id });

            // Act
            var result = await _matchService.CreateMatchAsync(request);

            // Assert
            _pointsServiceMock.Verify(ps =>
                ps.UpdateTeamsPointsBasedOnMatch(match, homeTeam, awayTeam, PointsAction.Apply),
                Times.Once);

            _unitOfWorkMock.Verify(u => u.Matches.AddAsync(match), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<MatchResponse>(match), Times.Once);

            Assert.AreEqual(match.Id, result.Id);
        }

        [Test]
        public async Task UpdateMatchAsync_ShouldRevertOldPoints_AndApplyNewPoints()
        {
            // Arrange
            var matchId = 1;

            var oldHome = new Team { Id = 1 };
            var oldAway = new Team { Id = 2 };

            var match = new Match
            {
                Id = matchId,
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeTeam = oldHome,
                AwayTeam = oldAway
            };

            var request = new MatchRequest { HomeTeamId = 3, AwayTeamId = 4 };

            var newHome = new Team { Id = 3 };
            var newAway = new Team { Id = 4 };

            _unitOfWorkMock.Setup(u => u.Matches.GetByConditionAsync(
                    It.IsAny<Expression<Func<Match, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(match);

            _unitOfWorkMock.SetupSequence(u => u.Teams.GetByConditionAsync(It.IsAny<Expression<Func<Team, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(newHome)
                .ReturnsAsync(newAway);

            _mapperMock.Setup(m => m.Map(request, match));

            // Act
            await _matchService.UpdateMatchAsync(matchId, request);

            // Assert
            _pointsServiceMock.Verify(ps =>
                ps.UpdateTeamsPointsBasedOnMatch(match, oldHome, oldAway, PointsAction.Revert),
                Times.Once);

            _pointsServiceMock.Verify(ps =>
                ps.UpdateTeamsPointsBasedOnMatch(match, newHome, newAway, PointsAction.Apply),
                Times.Once);

            _mapperMock.Verify(m => m.Map(request, match), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteMatchAsync_ShouldRevertPoints_AndDeleteMatch()
        {
            // Arrange
            var matchId = 10;

            var match = new Match
            {
                Id = matchId,
                HomeTeam = new Team { Id = 1 },
                AwayTeam = new Team { Id = 2 }
            };

            _unitOfWorkMock.Setup(u => u.Matches.GetByConditionAsync(
                    It.IsAny<Expression<Func<Match, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(match);

            // Act
            await _matchService.DeleteMatchAsync(matchId);

            // Assert
            _pointsServiceMock.Verify(ps =>
                ps.UpdateTeamsPointsBasedOnMatch(match, match.HomeTeam, match.AwayTeam, PointsAction.Revert),
                Times.Once);

            _unitOfWorkMock.Verify(u => u.Matches.Delete(match), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
