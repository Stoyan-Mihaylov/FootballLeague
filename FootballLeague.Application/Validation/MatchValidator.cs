using FluentValidation;
using FootballLeague.Application.Models.Matches;

namespace FootballLeague.Application.Validation
{
    public class MatchValidator : AbstractValidator<MatchRequest>
    {
        public MatchValidator()
        {
            RuleFor(x => x.HomeTeamId)
                .GreaterThan(0)
                .WithMessage("Valid HomeTeamId is required.");

            RuleFor(x => x.AwayTeamId)
                .GreaterThan(0)
                .WithMessage("Valid AwayTeamId is required.")
                .NotEqual(x => x.HomeTeamId)
                .WithMessage("Home & Away teams cannot be with the same id's.");

            RuleFor(x => x.HomeScore)
               .GreaterThanOrEqualTo(0)
               .WithMessage("Home score cannot be negative.");

            RuleFor(x => x.AwayScore)
               .GreaterThanOrEqualTo(0)
               .WithMessage("Away score cannot be negative.");
        }
    }
}
