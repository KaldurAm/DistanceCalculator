using System.Net;
using DistanceCalculator.Contracts;
using DistanceCalculator.Core.Calculator.Queries;
using DistanceCalculator.Evaluator.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DistanceCalculator.Api.Controllers.v1;

/// <summary>
/// provides the method to calculate distance between two iata codes
/// </summary>
[Route("api/v{version:apiVersion}/distance")]
[ApiVersion("1.0")]
public class DistanceController : BaseController
{
    private readonly ILogger<DistanceController> _logger;

    /// <inheritdoc />
    public DistanceController(ILogger<DistanceController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// calculates distance in miles
    /// </summary>
    /// <exception cref = "ArgumentOutOfRangeException"> </exception>
    [HttpGet("compute-in-miles")]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(DistanceContract))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.InternalServerError, type: typeof(ProblemDetails))]
    public async Task<IActionResult> ComputeInMiles(string originIata, string destinationIata)
    {
        var scope = new Dictionary<string, object> { { "Origin", originIata }, { "Destination", destinationIata }, };

        using (_logger.BeginScope(scope))
        {
            var resultOfGetDistance = await Mediator.Send(new CalculateDistanceQuery(
                originIata,
                destinationIata,
                DistanceUnit.MI));

            if (resultOfGetDistance.IsFailed)
                return ProblemResult(resultOfGetDistance.Error);

            return Ok(new DistanceContract(originIata,
                destinationIata,
                resultOfGetDistance.Value.Value,
                DistanceUnit.MI.ToString()));
        }
    }

    /// <summary>
    /// calculates distance in kilometers
    /// </summary>
    [HttpGet("compute-in-kilometers")]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(DistanceContract))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.InternalServerError, type: typeof(ProblemDetails))]
    public async Task<IActionResult> ComputeInKilometers(string originIata, string destinationIata)
    {
        var scope = new Dictionary<string, object> { { "IataCode", originIata }, { "Destination", destinationIata }, };

        using (_logger.BeginScope(scope))
        {
            var resultOfGetDistance = await Mediator.Send(new CalculateDistanceQuery(
                originIata,
                destinationIata,
                DistanceUnit.KM));

            if (resultOfGetDistance.IsFailed)
                return ProblemResult(resultOfGetDistance.Error);

            return Ok(new DistanceContract(originIata,
                destinationIata,
                resultOfGetDistance.Value.Value,
                DistanceUnit.KM.ToString()));
        }
    }
}