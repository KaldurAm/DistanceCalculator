using DistanceCalculator.Core.Airports.Queries;
using DistanceCalculator.Core.Common.Primitives;
using DistanceCalculator.Core.Interfaces.Services;
using DistanceCalculator.Core.ValueObjects;
using DistanceCalculator.Evaluator.Interfaces;
using DistanceCalculator.Results.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DistanceCalculator.Core.Calculator.Queries;

/// <summary>
/// handle command calculate distance
/// </summary>
public class CalculateDistanceQueryHandler : IRequestHandler<CalculateDistanceQuery, Result<ValueObjects.Distance>>
{
    private readonly ICacheService _cacheService;
    private readonly IIataDistanceCalculator _iataDistanceCalculator;
    private readonly ILogger<CalculateDistanceQueryHandler> _logger;
    private readonly ISender _sender;

    public CalculateDistanceQueryHandler(ILogger<CalculateDistanceQueryHandler> logger,
        IIataDistanceCalculator iataDistanceCalculator,
        ICacheService cacheService,
        ISender sender)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _iataDistanceCalculator = iataDistanceCalculator ?? throw new ArgumentNullException(nameof(iataDistanceCalculator));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    /// <inheritdoc />
    public async Task<Result<Distance>> Handle(CalculateDistanceQuery request, CancellationToken cancellationToken)
    {
        if (request.Origin.Equals(request.Destination))
        {
            _logger.LogWarning("The origin IATA code {Origin} and destination IATA code {Destination} are identical",
                request.Origin, request.Destination);
            return Result.Failure<Distance>(new Error(ErrorCode.CLIENT_ERROR,
                "The origin IATA code and destination IATA code are identical"));
        }

        var taskToGetOriginInformation = _sender.Send(new GetAirportInformationQuery(request.Origin), cancellationToken);
        var taskToGetDestinationInformation = _sender.Send(new GetAirportInformationQuery(request.Destination), cancellationToken);

        Task.WaitAll(new Task[] { taskToGetOriginInformation, taskToGetDestinationInformation, }, cancellationToken);

        if (taskToGetOriginInformation.Result.IsFailed || taskToGetDestinationInformation.Result.IsFailed)
        {
            _logger.LogError("One of the requests to get information about airports failed");
            return Result.Failure<Distance>(new Error(ErrorCode.SERVICE_ERROR,
                "One of the requests to get information about airports failed"));
        }

        var key = $"{request.Origin}:{request.Destination}";

        var distance = await _cacheService.GetAsync<double?>(key);

        if (distance is null)
        {
            _logger.LogInformation("Distance not found in cache by key {CacheKey}", key);

            var origin = taskToGetOriginInformation.Result.Value;
            var destination = taskToGetDestinationInformation.Result.Value;

            distance = _iataDistanceCalculator.Calculate(
                origin.Location.Lat, origin.Location.Lon,
                destination.Location.Lat, destination.Location.Lon,
                request.Unit);

            _cacheService.SetAsync(key, distance, TimeSpan.FromDays(1));

            _logger.LogInformation("Distance between {Origin} and {Destination} saved to memory cache ({Distance})",
                request.Origin,
                request.Destination,
                distance);
        }

        return Result.Success(new Distance(distance.Value));
    }
}