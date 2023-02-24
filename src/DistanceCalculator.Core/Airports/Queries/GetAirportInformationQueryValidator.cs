using FluentValidation;

namespace DistanceCalculator.Core.Airports.Queries;

public class GetAirportInformationQueryValidator : AbstractValidator<GetAirportInformationQuery>
{
    public GetAirportInformationQueryValidator()
    {
        RuleFor(x => x.Iata).NotNull().NotEmpty().MinimumLength(3).MaximumLength(3);
    }
}