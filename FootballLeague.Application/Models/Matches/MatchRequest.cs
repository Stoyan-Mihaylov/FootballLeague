using FootballLeague.Application.Mapping;
using FootballLeague.Domain.Entities;

namespace FootballLeague.Application.Models.Matches
{
    public class MatchRequest : IMapFrom<Match>
    {
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
    }
}
