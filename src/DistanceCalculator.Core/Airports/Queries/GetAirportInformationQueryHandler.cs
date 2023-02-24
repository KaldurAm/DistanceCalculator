using DistanceCalculator.Core.Common.Primitives;
using DistanceCalculator.Core.Contracts;
using DistanceCalculator.Core.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DistanceCalculator.Core.Airports.Queries;

/// <summary>
/// handle query retrieving information about airport by iata code
/// </summary>
public class GetAirportInformationQueryHandler : IRequestHandler<GetAirportInformationQuery, Result<Airport>>
{
    private readonly IAirportService _airportService;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetAirportInformationQueryHandler> _logger;

    public GetAirportInformationQueryHandler(ILogger<GetAirportInformationQueryHandler> logger,
        IAirportService airportService,
        ICacheService cacheService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _airportService = airportService ?? throw new ArgumentNullException(nameof(airportService));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    }

    /// <inheritdoc />
    public async Task<Result<Airport>> Handle(GetAirportInformationQuery request,
        CancellationToken cancellationToken)
    {
        var airport = await _cacheService.GetAsync<Airport>(request.Iata);

        if (airport is null)
        {
            _logger.LogInformation("Airport not found in cache by IATA code {IataCode}", request.Iata);

            var resultOfAirport = await _airportService.SendRequestToGetInformationAsync(request.Iata, cancellationToken);

            if (resultOfAirport.IsFailed)
            {
                _logger.LogError("Getting airport by IATA code failed");
                return resultOfAirport;
            }

            airport = resultOfAirport.Value;

            await _cacheService.SetAsync(request.Iata, airport, TimeSpan.FromDays(1));

            _logger.LogInformation("Airport saved to memory cache {@Airport}", airport);
        }

        return Result.Success(airport);
    }
}