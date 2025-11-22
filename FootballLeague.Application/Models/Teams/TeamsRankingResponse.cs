namespace FootballLeague.Application.Models.Teams
{
    public class TeamsRankingResponse
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int Played { get; set; }
        public int HomePlayed { get; set; }
        public int AwayPlayed { get; set; }
        public int Won { get; set; }
        public int Draw { get; set; }
        public int Lost { get; set; }
        public int Points { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
    }
}
