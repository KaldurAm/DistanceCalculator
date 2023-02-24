using DistanceCalculator.Core.Common.Primitives;
using DistanceCalculator.Core.Contracts;
using MediatR;

namespace DistanceCalculator.Core.Airports.Queries;

/// <summary>
/// query retrieving information about airport by iata code
/// </summary>
public class GetAirportInformationQuery : IRequest<Result<Airport>>
{
    public GetAirportInformationQuery(string iataCode)
    {
        Iata = iataCode.ToUpper();
    }

    public string Iata { get; }
}