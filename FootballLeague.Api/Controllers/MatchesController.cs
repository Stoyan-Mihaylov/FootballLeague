using FootballLeague.Application.Contracts.Services;
using FootballLeague.Application.Models.Matches;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public async Task<ActionResult<List<MatchResponse>>> GetAllMatches()
            => Ok(await _matchService.GetAllMatchesAsync());

        [HttpPost]
        public async Task<ActionResult<MatchResponse>> CreateMatch([FromBody] MatchRequest request)
        {
            var match = await _matchService.CreateMatchAsync(request);
            return CreatedAtAction(nameof(CreateMatch), match);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMatch(int id, [FromBody] MatchRequest request)
        {
            await _matchService.UpdateMatchAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMatch(int id)
        {
            await _matchService.DeleteMatchAsync(id);
            return NoContent();
        }
    }
}
