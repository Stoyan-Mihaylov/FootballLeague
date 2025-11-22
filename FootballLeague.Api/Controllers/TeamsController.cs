using FootballLeague.Application.Contracts.Services;
using FootballLeague.Application.Models.Teams;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public async Task<ActionResult<List<GetAllTeamsResponse>>> GetAllTeams()
            => Ok(await _teamService.GetAllTeamsAsync());

        [HttpGet("rankings")]
        public async Task<ActionResult<List<TeamsRankingResponse>>> GetRankings()
            => Ok(await _teamService.GetTeamsRankingAsync());

        [HttpPost]
        public async Task<ActionResult<TeamResponse>> CreateTeam([FromBody] TeamRequest request)
        {
            var team = await _teamService.CreateTeamAsync(request);
            return CreatedAtAction(nameof(CreateTeam), team);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTeam(int id, [FromBody] TeamRequest request)
        {
            await _teamService.UpdateTeamAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeam(int id)
        {
            await _teamService.DeleteTeamAsync(id);
            return NoContent();
        }
    }
}
