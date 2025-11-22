using FluentValidation;
using FootballLeague.Application.Models.Teams;

public class TeamValidator : AbstractValidator<TeamRequest>
{
    public TeamValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Team name is required.")
            .MaximumLength(100)
            .WithMessage("Team name cannot exceed 100 characters.");

        RuleFor(x => x.Points)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Points cannot be negative.");
    }
}
