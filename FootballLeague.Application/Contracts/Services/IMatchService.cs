using FootballLeague.Application.Models.Matches;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Application.Contracts.Services
{
    public interface IMatchService
    {
        Task<List<MatchResponse>> GetAllMatchesAsync(CancellationToken cancellationToken);
        Task<MatchResponse> CreateMatchAsync(MatchRequest request, CancellationToken cancellationToken);
        Task UpdateMatchAsync(int matchId, MatchRequest request, CancellationToken cancellationToken);
        Task DeleteMatchAsync(int matchId, CancellationToken cancellationToken);
    }
}
