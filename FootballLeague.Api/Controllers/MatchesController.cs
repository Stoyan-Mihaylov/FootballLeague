using FootballLeague.Application.Contracts.Services;
using FootballLeague.Application.DTOs;
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
        public async Task<ActionResult<List<GetAllMatchesDto>>> GetAllMatches()
            => Ok(await _matchService.GetAllMatchesAsync());

        [HttpPost]
        public async Task<ActionResult> CreateMatch([FromBody] MatchDto matchDto)
        {
            var match = await _matchService.CreateMatchAsync(matchDto);
            return CreatedAtAction(nameof(CreateMatch), match);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMatch(int id, [FromBody] MatchDto matchDto)
        {
            await _matchService.UpdateMatchAsync(id, matchDto);
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
