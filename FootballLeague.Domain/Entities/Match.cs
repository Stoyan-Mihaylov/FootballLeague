using FootballLeague.Domain.Enums;
using System;

namespace FootballLeague.Domain.Entities
{
    public class Match : BaseEntity<int>
    {
        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; }

        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
    }
}
