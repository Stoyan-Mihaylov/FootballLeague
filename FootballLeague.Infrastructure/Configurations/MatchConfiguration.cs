using FootballLeague.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballLeague.Infrastructure.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.HomeScore)
                   .IsRequired();

            builder.Property(m => m.AwayScore)
                   .IsRequired();

            builder.HasOne(m => m.HomeTeam)
                   .WithMany(t => t.HomeMatches)
                   .HasForeignKey(m => m.HomeTeamId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(m => m.AwayTeam)
                   .WithMany(t => t.AwayMatches)
                   .HasForeignKey(m => m.AwayTeamId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();
        }
    }
}
