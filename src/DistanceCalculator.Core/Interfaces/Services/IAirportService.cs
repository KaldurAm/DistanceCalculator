using DistanceCalculator.Core.Common.Primitives;
using DistanceCalculator.Core.Contracts;

namespace DistanceCalculator.Core.Interfaces.Services;

/// <summary>
/// Http client service adapter to send request for getting information about airport
/// </summary>
public interface IAirportService
{
    /// <summary>
    /// Send request to external service by rest and get from there information about airport by IATA code
    /// </summary>
    Task<Result<Airport>> SendRequestToGetInformationAsync(string iataCode, CancellationToken cancellationToken = default);
}