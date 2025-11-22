using FootballLeague.Application.Mapping;
using FootballLeague.Domain.Entities;

namespace FootballLeague.Application.Models.Matches
{
    public class MatchResponse : IMapFrom<Match>
    {
        public int Id { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
    }
}
