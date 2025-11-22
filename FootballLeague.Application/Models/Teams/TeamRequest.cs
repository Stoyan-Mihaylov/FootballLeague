using FootballLeague.Application.Mapping;
using FootballLeague.Domain.Entities;

namespace FootballLeague.Application.Models.Teams
{
    public class TeamRequest : IMapFrom<Team>
    {
        public string Name { get; set; }
        public int Points { get; set; }
    }
}
