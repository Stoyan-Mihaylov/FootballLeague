using FootballLeague.Application.Models.Matches;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballLeague.Application.Contracts.Services
{
    public interface IMatchService
    {
        Task<List<MatchResponse>> GetAllMatchesAsync();
        Task<MatchResponse> CreateMatchAsync(MatchRequest request);
        Task UpdateMatchAsync(int matchId, MatchRequest request);
        Task DeleteMatchAsync(int matchId);
    }
}
