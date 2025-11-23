using FootballLeague.Application.Mapping;
using FootballLeague.Domain.Entities;

namespace FootballLeague.Application.Models.Teams
{
    public class TeamResponse : IMapFrom<Team>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
    }
}
