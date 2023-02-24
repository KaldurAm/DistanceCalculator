using FluentValidation;

namespace DistanceCalculator.Core.Calculator.Queries;

public class CalculateDistanceQueryValidator : AbstractValidator<CalculateDistanceQuery>
{
    public CalculateDistanceQueryValidator()
    {
        RuleFor(x => x.Origin)
            .NotNull()
            .NotEmpty()
            .WithMessage("Origin IATA code can not be NULL or Empty");

        RuleFor(x => x.Destination)
            .NotNull()
            .NotEmpty()
            .WithMessage("Destination IATA code can not be NULL or Empty");
    }
}