using FootballLeague.Application.Contracts.Services;
using FootballLeague.Application.Models.Teams;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetAllTeamsResponse>>> GetAllTeams(CancellationToken cancellationToken)
            => Ok(await _teamService.GetAllTeamsAsync(cancellationToken));

        [HttpGet("rankings")]
        public async Task<ActionResult<List<TeamsRankingResponse>>> GetRankings(CancellationToken cancellationToken)
            => Ok(await _teamService.GetTeamsRankingAsync(cancellationToken));

        [HttpPost]
        public async Task<ActionResult<TeamResponse>> CreateTeam([FromBody] TeamRequest request, CancellationToken cancellationToken)
        {
            var team = await _teamService.CreateTeamAsync(request, cancellationToken);
            return CreatedAtAction(nameof(CreateTeam), team);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTeam(int id, [FromBody] TeamRequest request, CancellationToken cancellationToken)
        {
            await _teamService.UpdateTeamAsync(id, request, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeam(int id, CancellationToken cancellationToken)
        {
            await _teamService.DeleteTeamAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
