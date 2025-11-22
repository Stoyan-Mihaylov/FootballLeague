using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FootballLeague.Domain.Entities
{
    public class Team : BaseEntity<int>
    {
        public string Name { get; set; }
        public int Points { get; set; }

        [JsonIgnore]
        public ICollection<Match> HomeMatches { get; set; }

        [JsonIgnore]
        public ICollection<Match> AwayMatches { get; set; }
    }
}
