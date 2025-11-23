using FootballLeague.Application.Contracts.Services;
using FootballLeague.Application.Models.Matches;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchesController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MatchResponse>>> GetAllMatches(CancellationToken cancellationToken)
            => Ok(await _matchService.GetAllMatchesAsync(cancellationToken));

        [HttpPost]
        public async Task<ActionResult<MatchResponse>> CreateMatch([FromBody] MatchRequest request, CancellationToken cancellationToken)
        {
            var match = await _matchService.CreateMatchAsync(request, cancellationToken);
            return CreatedAtAction(nameof(CreateMatch), match);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMatch(int id, [FromBody] MatchRequest request, CancellationToken cancellationToken)
        {
            await _matchService.UpdateMatchAsync(id, request, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMatch(int id, CancellationToken cancellationToken)
        {
            await _matchService.DeleteMatchAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
