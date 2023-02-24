using DistanceCalculator.Api.Common.Constants;

namespace DistanceCalculator.Api.Features.Middlewares;

/// <summary>
/// middleware sets begin scope for request logging
/// </summary>
public class LoggingMiddleware
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly RequestDelegate _next;

    /// <summary>
    /// create <see cref="LoggingMiddleware"/> instance
    /// </summary>
    /// <param name="next"></param>
    /// <param name="loggerFactory"></param>
    public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _loggerFactory = loggerFactory;
    }

    /// <summary>
    /// use middleware
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext context)
    {
        var logger = _loggerFactory.CreateLogger<LoggingMiddleware>();

        var scope = new Dictionary<string, object> { { HeaderConstant.CorrelationId, context.Request.Headers[HeaderConstant.CorrelationId] }, };

        using (logger.BeginScope(scope))
            await _next(context);
    }
}