using FootballLeague.Domain.Entities;
using FootballLeague.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Infrastructure
{
    public class FootballLeagueDbContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }

        public DbSet<Match> Matches { get; set; }

        public FootballLeagueDbContext(DbContextOptions<FootballLeagueDbContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new MatchConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
