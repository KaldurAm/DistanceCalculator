using System.Diagnostics;
using DistanceCalculator.Core.Common.Primitives;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DistanceCalculator.Core.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("----- Start handling request {CommandName} {@RequestBody} {@DateTimeUtc}",
            typeof(TRequest).Name, request, DateTime.UtcNow);

        var sw = Stopwatch.StartNew();
        var response = await next();
        sw.Stop();

        if (response is Result { IsFailed: true, } result)
            _logger.LogError("Request failed {CommandName}, {@Error}, {@DateTimeUtc}",
                typeof(TRequest).Name, result.Error, DateTime.UtcNow);

        _logger.LogInformation("----- Finish handling request {CommandName} {@ResponseBody} {@DateTimeUtc} ({ElapsedTime} ms.)",
            typeof(TRequest).Name, response, DateTime.UtcNow, sw.ElapsedMilliseconds);

        return response;
    }
}