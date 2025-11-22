using AutoMapper;
using FootballLeague.Application.Contracts.Repositories;
using FootballLeague.Application.Contracts.Services;
using FootballLeague.Application.Models.Matches;
using FootballLeague.Domain.Entities;
using FootballLeague.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballLeague.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IPointsService _pointsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MatchService(
            IPointsService pointsService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _pointsService = pointsService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<MatchResponse>> GetAllMatchesAsync()
        {
            var matches = await _unitOfWork.Matches.GetAllAsync();
            return _mapper.Map<List<MatchResponse>>(matches);
        }

        public async Task<MatchResponse> CreateMatchAsync(MatchRequest request)
        {
            var homeTeam = await _unitOfWork.Teams.GetByConditionAsync(t => t.Id == request.HomeTeamId);
            var awayTeam = await _unitOfWork.Teams.GetByConditionAsync(t => t.Id == request.AwayTeamId);

            var match = _mapper.Map<Match>(request);

            _pointsService.UpdateTeamsPointsBasedOnMatch(match, homeTeam, awayTeam, PointsAction.Apply);

            await _unitOfWork.Matches.AddAsync(match);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<MatchResponse>(match);
        }

        public async Task UpdateMatchAsync(int matchId, MatchRequest request)
        {
            var match = await _unitOfWork.Matches.GetByConditionAsync(m => m.Id == matchId);

            _pointsService.UpdateTeamsPointsBasedOnMatch(match, match.HomeTeam, match.AwayTeam, PointsAction.Revert);

            var newHomeTeam = match.HomeTeamId != request.HomeTeamId
                ? await _unitOfWork.Teams.GetByConditionAsync(t => t.Id == request.HomeTeamId)
                : match.HomeTeam;

            var newAwayTeam = match.AwayTeamId != request.AwayTeamId
                ? await _unitOfWork.Teams.GetByConditionAsync(t => t.Id == request.AwayTeamId)
                : match.AwayTeam;

            _mapper.Map(request, match);
            _pointsService.UpdateTeamsPointsBasedOnMatch(match, newHomeTeam, newAwayTeam, PointsAction.Apply);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteMatchAsync(int matchId)
        {
            var match = await _unitOfWork.Matches.GetByConditionAsync(m => m.Id == matchId);

            _pointsService.UpdateTeamsPointsBasedOnMatch(match, match.HomeTeam, match.AwayTeam, PointsAction.Revert);

            _unitOfWork.Matches.Delete(match);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
