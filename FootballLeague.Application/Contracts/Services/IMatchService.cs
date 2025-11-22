using FootballLeague.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballLeague.Application.Contracts.Services
{
    public interface IMatchService
    {
        Task<List<GetAllMatchesDto>> GetAllMatchesAsync();
        Task<CreateMatchDto> CreateMatchAsync(MatchDto matchDto);
        Task UpdateMatchAsync(int matchId, MatchDto matchDto);
        Task DeleteMatchAsync(int matchId);
    }
}
