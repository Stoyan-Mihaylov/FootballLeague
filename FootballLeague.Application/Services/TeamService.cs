using AutoMapper;
using FootballLeague.Application.Contracts.Repositories;
using FootballLeague.Application.Contracts.Services;
using FootballLeague.Application.DTOs;
using FootballLeague.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballLeague.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeamService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GetAllTeamsDto>> GetAllTeamsAsync()
        {
            var teams = await _unitOfWork.Teams.GetAllAsync();
            return _mapper.Map<List<GetAllTeamsDto>>(teams);
        }

        public async Task<List<TeamRankingDto>> GetTeamsRankingAsync()
            => await _unitOfWork.Teams.GetTeamsRankingAsync();

        public async Task<TeamDto> CreateTeamAsync(TeamDto teamDto)
        {
            var team = _mapper.Map<Team>(teamDto);
            await _unitOfWork.Teams.AddAsync(team);
            await _unitOfWork.SaveChangesAsync();
            return teamDto;
        }

        public async Task UpdateTeamAsync(int teamId, TeamDto teamDto)
        {
            var team = await _unitOfWork.Teams.GetByConditionAsync(t => t.Id == teamId);

            _mapper.Map(teamDto, team);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTeamAsync(int teamId)
        {
            var team = await _unitOfWork.Teams.GetByConditionAsync(t => t.Id == teamId);

            _unitOfWork.Teams.Delete(team);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
