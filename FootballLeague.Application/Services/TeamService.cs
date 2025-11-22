using AutoMapper;
using FootballLeague.Application.Contracts.Repositories;
using FootballLeague.Application.Contracts.Services;
using FootballLeague.Application.Models.Teams;
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

        public async Task<List<GetAllTeamsResponse>> GetAllTeamsAsync()
        {
            var teams = await _unitOfWork.Teams.GetAllAsync();
            return _mapper.Map<List<GetAllTeamsResponse>>(teams);
        }

        public async Task<List<TeamsRankingResponse>> GetTeamsRankingAsync()
            => await _unitOfWork.Teams.GetTeamsRankingAsync();

        public async Task<TeamResponse> CreateTeamAsync(TeamRequest request)
        {
            var team = _mapper.Map<Team>(request);
            await _unitOfWork.Teams.AddAsync(team);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TeamResponse>(team);
        }

        public async Task UpdateTeamAsync(int teamId, TeamRequest request)
        {
            var team = await _unitOfWork.Teams.GetByConditionAsync(t => t.Id == teamId);

            _mapper.Map(request, team);
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
