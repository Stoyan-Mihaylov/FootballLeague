using AutoMapper;
using FootballLeague.Application.Contracts.Repositories;
using FootballLeague.Application.Contracts.Services;
using FootballLeague.Application.Exceptions;
using FootballLeague.Application.Models.Teams;
using FootballLeague.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
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

        public async Task<List<GetAllTeamsResponse>> GetAllTeamsAsync(CancellationToken cancellationToken)
        {
            var teams = await _unitOfWork.Teams.GetAllAsync(cancellationToken);
            return _mapper.Map<List<GetAllTeamsResponse>>(teams);
        }

        public async Task<List<TeamsRankingResponse>> GetTeamsRankingAsync(CancellationToken cancellationToken)
            => await _unitOfWork.Teams.GetTeamsRankingAsync(cancellationToken);

        public async Task<TeamResponse> CreateTeamAsync(TeamRequest request, CancellationToken cancellationToken)
        {
            var team = _mapper.Map<Team>(request);
            await _unitOfWork.Teams.AddAsync(team, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<TeamResponse>(team);
        }

        public async Task UpdateTeamAsync(int teamId, TeamRequest request, CancellationToken cancellationToken)
        {
            var team = await _unitOfWork.Teams.GetByConditionAsync(t => t.Id == teamId, cancellationToken);

            _mapper.Map(request, team);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteTeamAsync(int teamId, CancellationToken cancellationToken)
        {
            bool isUsedInMatches = await _unitOfWork.Matches.ExistsAsync(
                m => m.HomeTeamId == teamId || m.AwayTeamId == teamId, 
                cancellationToken);

            if (isUsedInMatches)
                throw new BadRequestException("Cannot delete team because it is used in existing match.");

            var team = await _unitOfWork.Teams.GetByConditionAsync(t => t.Id == teamId, cancellationToken);

            _unitOfWork.Teams.Delete(team);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
