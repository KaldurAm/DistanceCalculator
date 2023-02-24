using System.Diagnostics;
using DistanceCalculator.Core.Common.Extensions;
using DistanceCalculator.Core.Common.Primitives;
using DistanceCalculator.Core.Contracts;
using DistanceCalculator.Core.Interfaces.Services;
using DistanceCalculator.Core.Options;
using DistanceCalculator.Core.ValueObjects;
using DistanceCalculator.Results.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DistanceCalculator.Infrastructure.Services;

internal class AirportService : IAirportService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly AirportServiceOptions _clientOptions;
    private readonly ILogger<AirportService> _logger;

    public AirportService(ILogger<AirportService> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<AirportServiceOptions> clientOptions)
    {
        _logger = logger;
        _clientOptions = clientOptions.Value;
        _clientFactory = httpClientFactory;
    }

    /// <inheritdoc />
    public async Task<Result<Airport>> SendRequestToGetInformationAsync(string iataCode, CancellationToken cancellationToken = default)
    {
        if (new Random().Next(1, 5).Equals(3))
            return Result.Failure<Airport>(new Error(ErrorCode.SERVICE_ERROR, "Problem when connect to service"));

        var stopwatch = Stopwatch.StartNew();

        HttpResponseMessage httpResponse;

        try
        {
            var client = _clientFactory.CreateClient(nameof(AirportService));
            httpResponse = await client.GetAsync($"{_clientOptions.Endpoint}/{iataCode}", cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error occured while sending request to get information about airport by IATA code");
            return Result.Failure<Airport>(new Error(ErrorCode.SERVICE_ERROR,
                "Error occured while sending request to get information about airport by IATA code"));
        }

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogWarning("Request sent to airport information service failed");
            return Result.Failure<Airport>(new Error(ErrorCode.REQUEST_ERROR,
                "Request sent to airport information service failed"));
        }

        var content = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

        if (content.IsNullOrEmpty())
        {
            _logger.LogWarning("HTTP response content is null or empty");
            return Result.Failure<Airport>(new Error(ErrorCode.CONTENT_ERROR,
                "HTTP response content is null or empty"));
        }

        var iataInformation = content.ConvertTo<Airport>();

        if (iataInformation is null)
        {
            _logger.LogWarning("HTTP response content deserialization failed");
            return Result.Failure<Airport>(new Error(ErrorCode.SERIALIZATION_ERROR,
                "HTTP response content deserialization failed"));
        }

        stopwatch.Stop();

        _logger.LogInformation("HTTP request executed in ({ElapsedTime} ms.)", stopwatch.ElapsedMilliseconds);

        return Result.Success(iataInformation);
    }
}