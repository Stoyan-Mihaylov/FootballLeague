using AutoMapper;
using FootballLeague.Application.Contracts.Repositories;
using FootballLeague.Application.Exceptions;
using FootballLeague.Application.Models.Teams;
using FootballLeague.Application.Services;
using FootballLeague.Domain.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Match = FootballLeague.Domain.Entities.Match;

namespace FootballLeague.Application.Tests.Services
{
    [TestFixture]
    public class TeamServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private TeamService _teamService;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _teamService = new TeamService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetAllTeamsAsync_ShouldReturnMappedTeams()
        {
            // Arrange
            var teams = new List<Team>
            {
                new Team { Id = 1, Name = "Team1", Points = 1 },
                new Team { Id = 2, Name = "Team2", Points = 2 }
            };

            _unitOfWorkMock.Setup(u => u.Teams.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(teams);

            var mappedTeams = new List<GetAllTeamsResponse>
            {
                new GetAllTeamsResponse { Id = 1, Name = "Team1", Points = 1 },
                new GetAllTeamsResponse { Id = 2, Name = "Team2", Points = 2 }
            };

            _mapperMock.Setup(m => m.Map<List<GetAllTeamsResponse>>(teams))
                .Returns(mappedTeams);

            // Act
            var result = await _teamService.GetAllTeamsAsync(It.IsAny<CancellationToken>());

            // Assert
            Assert.AreEqual(mappedTeams, result);

            _unitOfWorkMock.Verify(u => u.Teams.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map<List<GetAllTeamsResponse>>(teams), Times.Once);
        }

        [Test]
        public async Task GetTeamsRankingAsync_ShouldReturnRepositoryResult()
        {
            // Arrange
            var expectedRanking = new List<TeamsRankingResponse>
            {
                new TeamsRankingResponse { TeamId = 1, TeamName = "TeamA", Points = 4 },
                new TeamsRankingResponse { TeamId = 2, TeamName = "TeamB", Points = 4 },
                new TeamsRankingResponse { TeamId = 3, TeamName = "TeamC", Points = 3 }
            };

            _unitOfWorkMock
                .Setup(u => u.Teams.GetTeamsRankingAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedRanking);

            // Act
            var result = await _teamService.GetTeamsRankingAsync(It.IsAny<CancellationToken>());

            // Assert
            Assert.AreEqual(expectedRanking, result);
            _unitOfWorkMock.Verify(u => u.Teams.GetTeamsRankingAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task CreateTeamAsync_ShouldAddTeamAndReturnResponse()
        {
            // Arrange
            var request = new TeamRequest { Name = "A" };
            var team = new Team { Id = 5, Name = "A" };

            var teamRepoMock = new Mock<ITeamRepository>();

            _unitOfWorkMock.Setup(u => u.Teams).Returns(teamRepoMock.Object);

            _mapperMock.Setup(m => m.Map<Team>(request))
                .Returns(team);

            teamRepoMock.Setup(r => r.AddAsync(It.IsAny<Team>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<TeamResponse>(team))
                .Returns(new TeamResponse { Id = 5, Name = "A" });

            // Act
            var result = await _teamService.CreateTeamAsync(request, It.IsAny<CancellationToken>());

            // Assert
            teamRepoMock.Verify(r => r.AddAsync(team, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreEqual(team.Id, result.Id);
            Assert.AreEqual(team.Name, result.Name);
            Assert.AreEqual(team.Points, result.Points);
        }

        [Test]
        public async Task UpdateTeamAsync_ShouldMapAndSave()
        {
            // Arrange
            var teamId = 7;
            var existingTeam = new Team { Id = teamId, Name = "OldName", Points = 7 };
            var request = new TeamRequest { Name = "NewName", Points = 3 };

            _unitOfWorkMock.Setup(u => u.Teams.GetByConditionAsync(It.IsAny<Expression<Func<Team, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                .ReturnsAsync(existingTeam);

            // Act
            await _teamService.UpdateTeamAsync(teamId, request, It.IsAny<CancellationToken>());

            // Assert
            _mapperMock.Verify(m => m.Map(request, existingTeam), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void DeleteTeamAsync_ShouldThrow_WhenTeamUsedInMatches()
        {
            // Arrange
            var teamId = 3;

            _unitOfWorkMock.Setup(u => u.Matches.ExistsAsync(It.IsAny<Expression<Func<Match, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act & Assert
            Assert.ThrowsAsync<BadRequestException>(async () =>
                await _teamService.DeleteTeamAsync(teamId, It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task DeleteTeamAsync_ShouldDeleteTeam_WhenNotUsedInMatches()
        {
            // Arrange
            var teamId = 3;
            var team = new Team { Id = teamId };

            _unitOfWorkMock.Setup(u => u.Matches.ExistsAsync(It.IsAny<Expression<Func<Match, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _unitOfWorkMock.Setup(u => u.Teams.GetByConditionAsync(It.IsAny<Expression<Func<Team, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                .ReturnsAsync(team);

            // Act
            await _teamService.DeleteTeamAsync(teamId, It.IsAny<CancellationToken>());

            // Assert
            _unitOfWorkMock.Verify(u => u.Teams.Delete(team), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
